//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
/// <summary>
/// Summary : Game communication client.Mainly responsible for the server and socket communications
/// Author: Liu Aifei (329737941@qq.com)
/// Data: 2015.8.3
/// </summary>
public class Client : SingletonComponent<Client>
{
    class QueueItem
    {
        public byte Flag { get; private set; }
        public int TipCode { get; private set; }
        public short MessageId { get; private set; }
        public byte[] Body { get; private set; }
        public SocketMessageHandler Handler { get; private set; }
        public QueueItem(short messageId,byte[] body ,SocketMessageHandler handler)
        {
            MessageId = messageId;
            Body = body;
            Handler = handler;
        }
    }
    public static short SERIAL = 0;
    private UnitySocket _unitySocket;
    private List<QueueItem> _receive;
    private List<QueueItem> _addReceive;
    private List<QueueItem> _removeReceive;

    private ClientSender _clientSender = null;
    public ClientSender ClientSender { get { return _clientSender; } }
    private ClientReceiver _clientReceiver = null;
    public ClientReceiver ClientReceiver { get { return _clientReceiver; } }

    private bool isExecute = false;
    public SocketCompleteHandler OnConnectHandler;

    private string _gameIp;
    private int _gamePort;

    private bool isInited = false;
    private ConnectionState ConnectionState;

    public bool isLogin = false;

    public Client() {
        _unitySocket = new UnitySocket();
        _receive = new List<QueueItem>();
        _addReceive = new List<QueueItem>();
        _removeReceive = new List<QueueItem>();
        _clientSender = new ClientSender(this);
        _clientReceiver = new ClientReceiver(this);
    }

    void Update() {
        if (_addReceive.Count > 0) {
            foreach (QueueItem item in _addReceive) {
                _receive.Add(item);
            }
            _addReceive.Clear();
        }

        if (_removeReceive.Count > 0) {
            foreach (QueueItem item in _removeReceive) {
                if (_receive.Contains(item)) {
                    _receive.Remove(item);
                }
            }
            _removeReceive.Clear();
        }

        if (_receive.Count > 0) {
            foreach (QueueItem item in _receive) {
                SocketMessageHandler handler = item.Handler;
                if (handler != null) {
                    try {
                        handler(item.MessageId, item.Body);
                    } catch (System.Exception ex) {
                        Debug.LogError(string.Format("To perform distributed message error, dislocation position location: {0},errer info :{1}", handler.ToString(), ex.ToString()));
                    }
                }
            }
            _receive.Clear();
        }

        if (isExecute && OnConnectHandler != null) {
            isExecute = false;
            OnConnectHandler(ConnectionState);
            if (ConnectionState == global::ConnectionState.ReConnected) {
                ReConnect();
            }
        }
    }

    public void Connect(string ip, int port) {
        _clientReceiver.RegisterPacket();
        _unitySocket.OnConnectComplete = OnConnectComplete;
        _unitySocket.OnCallBack = ReceiveCallBack;
        _unitySocket.Connect(ip, port);
        _gameIp = ip;
        _gamePort = port;
    }

    private void OnConnectComplete(ConnectionState state) {
        isExecute = true;
        ConnectionState = state;
        //if (state == ConnectionState.Connected) {
            
        //} else if (state == ConnectionState.ConnectTimeOut) {
        //    Debug.LogError(string.Format("Connect to the server timeout, IP = {0}, port = {1}", _gameIp, _gamePort));
        //} else {

        //}

    }
    private void ReConnect() {
        Debug.Log("Reconnected");
        isLogin = false;

        //ClientSender.LoginGame();
    }

    private void ReceiveCallBack(short messageId, byte[] body, SocketMessageHandler handler) {
        QueueItem queue = new QueueItem(messageId, body, handler);
        _addReceive.Add(queue);
    }

    public void Close() {
        _unitySocket.Close();
    }
    public void Send(Packet body) {
        if (_unitySocket.IsConnected) {
            _unitySocket.Send(body);
        } else {
            Debug.LogError("Server connection failure, cannot send data!!");
            //Servicer servicer = DataManager.ServicerInfo.SelectServicer;
            _unitySocket.Close();
//            if (servicer == null) {
//                Log.err("select server is null");
//            } else {
//                //_unitySocket.ReConnect(servicer.Host, servicer.Port);
//            }
        }
    }

    public void RegisterMessage(short messageId, SocketMessageHandler handler) {
        _unitySocket.RegisterMessage(messageId, handler);
    }
    public void RemoveMessage(short messageId) {
        _unitySocket.RemoveMessage(messageId);
    }
    public void HttpConnect(string url, Action<string> getResponse) {
        //Debug.Log("url = " + url);
        //UnityHttpClient httpClient = UnityCommunicationManager.CreateInstance().GetHttpClient();
        //httpClient.Error += HttpError;
        //httpClient.BeginGetHttpContent(url, getResponse);
        StartCoroutine(SendRequestAsync(url, getResponse));
    }
    public void HttpError(string error) {
        string message = string.Format("链接失败 : {0}", error);
        Debug.Log("HttpError:" + message);
        //UIManager.AlertView(AlertView.AlertType.Level3, message);
    }

    private IEnumerator SendRequestAsync(string url, Action<string> request) {
        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error)) {
            request(www.text);
            www.Dispose();
        } else {
            HttpError(www.error);
        }
    }

}