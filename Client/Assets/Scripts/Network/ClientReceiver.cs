//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Summary : Processing of messages received from the server
/// Author: Liu Aifei (329737941@qq.com)
/// Data: 2015.10.26
/// </summary>
public class ClientReceiver
{
    protected Client _client;

    public ClientReceiver(Client client) {
        _client = client;
    }

    #region

    public virtual void RegisterPacket() {
        MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
        foreach (MethodInfo method in methods) {
            PacketAttribute[] attributes = method.GetCustomAttributes(typeof(PacketAttribute), false) as PacketAttribute[];
            if (attributes.Length == 0)
                continue;

            try {
                SocketMessageHandler handlerDelegate = (SocketMessageHandler)Delegate.CreateDelegate(typeof(SocketMessageHandler), this, method);
                foreach (PacketAttribute attribute in attributes) {
                    _client.RegisterMessage(attribute.MessageId, handlerDelegate);
                }
            } catch (Exception e) {
                string handlerStr = this.GetType().FullName + "." + method.Name;
                throw new Exception("Unable to register PacketHandler " + handlerStr + ".\n" + e.Message);
            }
        }
    }

    #endregion

    //================================================================================================================================

}