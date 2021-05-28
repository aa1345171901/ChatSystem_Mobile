using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ClientManager : BaseManager
{
    private const string IP = "47.119.134.12"; // 47.119.134.12
    private const int PORT = 8888;

    // 用于进行数据处理
    private Message message;
    // 当前客户端的socket
    private Socket clientSocket;

    public ClientManager(Facade facade) : base(facade)
    {
    }

    /// <summary>
    /// 初始化，连接进入服务器
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            StartReceive();
        }
        catch (System.Exception e)
        {
            Debug.Log("无法连接服务器，请减查您的网络:" + e.Message);
        }
    }

    /// <summary>
    /// 开始接收服务器消息
    /// </summary>
    private void StartReceive()
    {
        clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, ReceveCallBack, null);
    }

    /// <summary>
    /// 接收消息的回调，一次消息接受完，再重新接收
    /// </summary>
    private void ReceveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }

            int count = clientSocket.EndReceive(ar);

            message.ReadMessage(count, OnProcessCallBack);
            StartReceive();
        }
        catch (Exception e)
        {
            Close();
            Debug.Log("异步回调出错:" + e.Message);
        }
    }

    /// <summary>
    /// 对接收的消息进行处理
    /// </summary>
    private void OnProcessCallBack(ActionCode actionCode, string data)
    {
        facade.HandleResponse(actionCode, data);
    }

    /// <summary>
    /// 将数据打包发送给sever
    /// </summary>
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        if (clientSocket != null)
        {
            clientSocket.Send(Message.PackData(requestCode, actionCode, data));
        }
    }

    /// <summary>
    /// 关闭时释放资源
    /// </summary>
    public void Close()
    {
        if (clientSocket != null)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            Thread.Sleep(10);
            clientSocket.Close();
            clientSocket.Dispose();
        }
    }

    public override void OnDestroy()
    {
        Close();
        base.OnDestroy();
    }
}
