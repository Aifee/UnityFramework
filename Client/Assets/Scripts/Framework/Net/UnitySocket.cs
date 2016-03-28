//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using ProtoBuf;

public enum ConnectionState
{
    NotConnect = 0,
    Connected = 2,
    ReConnected = 3,
    ConnectTimeOut = 4,
}
public enum ReadState {
    Head,
    Continued,
    End,
}
public delegate void SocketCompleteHandler(ConnectionState state);
public delegate void SocketMessageHandler(short messageId, byte[] body);
public delegate void SocketCallBackHandler(short messageId, byte[] body, SocketMessageHandler handler);
public delegate void SocketErrorHandler(string msg);
/// <summary>
/// @Summary : Network connection core processing
/// @Date : 2015.8.3
/// </summary>
public class UnitySocket {
    public const int HEARTBEATINTERVAL = 1000;
    public const int BUFFSIZE = 1024;

    public SocketCompleteHandler OnConnectComplete;
    public SocketErrorHandler OnError;
    public SocketCallBackHandler OnCallBack;
    private Dictionary<short, SocketMessageHandler> _receives;
    private Thread _checkThread;
    private bool _isRunning;
    private byte[] _buffer;
    private int _offset;
    private int _size;
    private string _ip;
    private int _port;
    private ReadState _state;
    private int _buffSize;
    private short _serial = 0;
    private List<Packet> _sendList = new List<Packet>();
    private IPAddress ipAddress;
    private IPEndPoint ipe;
    private Socket _socket;

    private bool isReConnect = false;
    public bool IsConnected { get { return ((this._socket != null) && this._socket.Connected); } }
    public bool IsClosed { get { return (this._socket == null); } }

    public UnitySocket() {
        _receives = new Dictionary<short, SocketMessageHandler>();
        isReConnect = false;
    }
    ~UnitySocket() {
        this.Close();
        _receives.Clear();
    }

    public void Connect(string ip, int port) {
        try {
            Close();
            _ip = ip;
            _port = port;
            ipAddress = IPAddress.Parse(ip);
            ipe = new IPEndPoint(ipAddress, port);
            _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.NoDelay = true;
            IAsyncResult result = _socket.BeginConnect(ipe, new AsyncCallback(this.OnConnect), this._socket);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success) {
                Close();
                Debug.Log("connect Time Out");
            }
        } catch (Exception exception) {
            Debug.LogError("Connect: " + exception.ToString());
            Error("k3432");
        }
    }
    public void ReConnect(string ip, int port) {
        isReConnect = true;
        Connect(ip,port);
    }
    private void OnConnect(IAsyncResult ret) {
        if (ret.AsyncState == _socket) {
            try {
                _socket.EndConnect(ret);
                ReadHead();
                if (OnConnectComplete != null) {
                    OnConnectComplete(isReConnect ? ConnectionState.ReConnected : ConnectionState.Connected);
                }
            } catch (Exception exception) {
                Debug.Log(exception);
            }
        }
    }

    private void ReadHead() {
        _state = ReadState.Head;
        _size = 4;
        _offset = 0;
        _buffer = new byte[this._size];
        ContinueRead();
    }

    private void ReadData(int pack_len) {
        _size = pack_len;
        //_buffer = new byte[_size];
        ContinueRead();
    }
    private void ContinueRead() {
        try {
            if (this.IsConnected) {
                _socket.BeginReceive(_buffer, _offset, _size - _offset, SocketFlags.None, new AsyncCallback(OnRead), null);
            }
        } catch (Exception exception) {
            Debug.LogError(exception.ToString());
            Error("k3432");
        }
    }
    private void OnRead(IAsyncResult ar) {
        try {
            if (IsConnected) {
                int endRe = _socket.EndReceive(ar);
                _offset += endRe;
                //Debug.Log(string.Format("endRe :{0}_offset ={1},_size = {2} ", endRe, _offset, _size));
                if (endRe > 0) {
                    if (_state == ReadState.Head) {
                        Array.Reverse(_buffer, 0, 4);
                        _buffSize = BitConverter.ToInt32(_buffer, 0);
                        _buffer = new byte[_buffSize];
                        _state = ReadState.Continued;
                    }
                    if (_state == ReadState.Continued) {
                        //Debug.Log(string.Format("offset : {0},size : {1},buffSize : {2}", _offset, _size, _buffSize));
                        if (_offset == _buffSize) {
                            _state = ReadState.End;
                        } else {
                            ReadData(_buffSize);
                        }
                    }
                    if (_state == ReadState.End) {
                        ParsePacket(_buffer);
                        ReadHead();
                    }
                } else {
                    ContinueRead();
                }
            }
        } catch (Exception exception) {
            Debug.Log(exception);
        }
    }
    private void ParsePacket(byte[] buff) {
        int offset = 10;
        Array.Reverse(buff, 4, 2);
        short serial = BitConverter.ToInt16(buff, 4);
        byte version = buff[6];
        byte encryption = buff[7];
        Array.Reverse(buff, 8, 2);
        short messageId = BitConverter.ToInt16(buff, 8);
        int size = buff.Length;
        byte[] data = new byte[size - offset];
        Array.Copy(buff, offset, data, 0, size - offset);
        //Debug.Log("messageId = " + messageId);
        if (_receives.ContainsKey(messageId) && OnCallBack != null) {
            OnCallBack(messageId, data, _receives[messageId]);
        } else {
            Debug.LogError(string.Format("No registration concerns the server returned to the message {0},serial:{1},version:{2},encryption:{3},size:{4} ", messageId, serial, version, encryption, size));
        }
    }
    private void OnSend(IAsyncResult ar) {
        Packet asyncState = ar.AsyncState as Packet;
        Socket sock = asyncState.Socket;
        if (sock.Connected) {
            sock.EndSend(ar);
            List<Packet> list = _sendList;
            lock (list) {
                if (_sendList.Contains(asyncState)) {
                    _sendList.Remove(asyncState);
                }
            }
            ContinueSend();
        }
    }

    public void Send(Packet packet) {
        if (this.IsConnected) {
            packet.IsStart = false;
            packet.Socket = _socket;
            int count = 0;
            List<Packet> list = _sendList;
            lock (list) {
                _sendList.Add(packet);
                count = _sendList.Count;
            }
            if (count > 0x3e8) {

            } else {
                ContinueSend();
            }
        }
    }
    private void ContinueSend() {
        Packet packet = null;
        List<Packet> list = _sendList;
        lock (list) {
            if (_sendList.Count == 0) {
                return;
            }
            packet = _sendList[0];
            if (packet.IsStart) {
                return;
            }
            packet.IsStart = true;
        }
        //Debug.Log("send packet id :" + packet.MessageId);
        Socket socket = packet.Socket;
        if (socket.Connected) {
            packet.Serial = ++_serial;
            byte[] packetbuff = packet.GetBuffer();
            if (packetbuff != null) {
                socket.BeginSend(packetbuff, 0, packetbuff.Length, SocketFlags.None, new AsyncCallback(OnSend), packet);
            }
        }
    }

    public void StartHeartbeat() {
        if (_checkThread == null) {
            _checkThread = new Thread(new ThreadStart(CheckServerThread));
        }
    }
    private void CheckServerThread() {
        while (_isRunning) {
            if (IsConnected) {
                ////Send heartbeat packet
            } else {
                ////Re connect

            }
            Thread.Sleep(HEARTBEATINTERVAL);
        }
    }

    public int GetSendQueueSize() {
        List<Packet> list = _sendList;
        lock (list) {
            return _sendList.Count;
        }
    }

    public void Close() {
        try {
            Socket socket = _socket;
            this.Clear();
            _isRunning = false;
            if ((socket != null) && socket.Connected) {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        } catch (Exception exception) {
            Debug.LogError("Connect: " + exception.ToString());
            this.Error("k3432");
        }
    }

    private void Clear() {
        _ip = null;
        _port = 0;
        _socket = null;
        List<Packet> list = _sendList;
        lock (list) {
            _sendList.Clear();
        }
        _buffer = null;
        _offset = 0;
        _size = 0;
        if (_checkThread != null && _checkThread.IsAlive)
            _checkThread.Abort();

    }
    protected void Error(string msg) {
        Close();
        if (OnError != null)
            OnError(msg);
    }

    public void RegisterMessage(short messageId, SocketMessageHandler handler) {
        if (_receives.ContainsKey(messageId)) {
            _receives[messageId] = handler;
        } else {
            _receives.Add(messageId, handler);
        }
    }
    public void RemoveMessage(short messageId, bool isSpecial = false) {
        if (_receives.ContainsKey(messageId)) {
            _receives.Remove(messageId);
        } else {
            Debug.Log("No news of registered :" + messageId);
        }
    }


    public static byte[] Serialize(IExtensible msg) {
        byte[] result;
        using (MemoryStream stream = new MemoryStream()) {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
        }
        return result;
    }

    public static T Deserialize<T>(byte[] message) where T : IExtensible {
        T result;
        using (MemoryStream stream = new MemoryStream(message)) {
            result = Serializer.Deserialize<T>(stream);
        }
        if (result == null) {
            Debug.LogError("Parse error message body!!");
        }
        return result;
    }

}
