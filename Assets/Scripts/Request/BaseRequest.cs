using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour
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
    /// Awake生命周期，将该request存入requestManager字典
    /// </summary>
    public virtual void Awake()
    {
        Facade.AddRequest(actionCode, this);
    }

    /// <summary>
    /// 物体销毁，将该Request也从字典移除
    /// </summary>
    public virtual void OnDestroy()
    {
        Facade.RemoveRequest(actionCode);
    }

    /// <summary>
    /// 对收到的信息进行处理
    /// </summary>
    public virtual void OnResponse(string data)
    {
    }

    /// <summary>
    /// 项服务器发送请求，通过_facade发送
    /// </summary>
    public virtual void SendRequest(string data)
    {
        Facade.SendRequest(requestCode, actionCode, data);
    }
}
