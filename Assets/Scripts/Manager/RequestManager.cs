using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于管理所有request，用于获得队友request类
/// </summary>
public class RequestManager : BaseManager
{
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public RequestManager(Facade facade) : base(facade)
    {
    }

    /// <summary>
    /// 添加字典操作，通过单例调用
    /// </summary>
    public void AddRequestDic(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    /// <summary>
    /// 移除字典操作，通过单例调用
    /// </summary>
    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }

    /// <summary>
    /// 找出对应的request对消息进行响应
    /// </summary>
    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestDict.TryGetValue(actionCode, out BaseRequest request);
        if (request == null)
        {
            Debug.Log("无法得到ActionCode[" + actionCode + "]对应的类");
            return;
        }

        request.OnResponse(data);
    }
}
