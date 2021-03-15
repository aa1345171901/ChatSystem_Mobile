using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRequest
{
    protected ActionCode actionCode = ActionCode.None;
    protected RequestCode requestCode = RequestCode.None;
    protected Facade _facade;

    protected Facade Facade
    {
        get
        {
            if (_facade == null)
            {
                _facade = Facade.Instance;
            }

            return _facade;
        }
    }

    /// <summary>
    /// 初始化，将该request存入requestManager字典
    /// </summary>
    public virtual void Init()
    {
        _facade.AddRequest(actionCode, this);
    }

    /// <summary>
    /// 该form关闭时，将该Request也从字典移除
    /// </summary>
    public virtual void Close()
    {
        _facade.RemoveRequest(actionCode);
    }

    /// <summary>
    /// 对收到的信息进行处理
    /// </summary>
    public abstract void OnResponse(string data);

    /// <summary>
    /// 项服务器发送请求，通过managerController发送
    /// </summary>
    public abstract void SendRequest(string data);
}
