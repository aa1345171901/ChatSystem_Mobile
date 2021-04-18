using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式，用于管理所有的manager，全局只有一个
/// </summary>
public class Facade : MonoBehaviour
{
    private static Facade _instance;

    private ClientManager clientManager;
    private UIManager uiManager;
    private RequestManager requestManager;
    private UserManager userManager;

    private bool isGet = false;   // 判断好友资料是否获得
    public bool IsGet
    {
        get
        {
            return IsGet;
        }
        set
        {
            isGet = value;
        }
    }

    /// <summary>
    /// 单例模式，全局只有一个Facade控制
    /// </summary>
    public static Facade Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// 在unity生命周期start前，给单例_instance赋值
    /// </summary>
    private void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            new GameObject().AddComponent<Facade>();
            _instance = this;
        }
    }

    private void Start()
    {
        OnInit();
    }

    /// <summary>
    /// 实现Manager的update，需要异步生成物体什么的
    /// </summary>
    public void Update()
    {
        clientManager.Update();
        uiManager.Update();
        requestManager.Update();
        userManager.Update();
    }

    /// <summary>
    /// Manager初始化，在Start周期进行，在Awake之后，保证_instance存在
    /// </summary>
    public void OnInit()
    {
        clientManager = new ClientManager(this);
        uiManager = new UIManager(this);
        requestManager = new RequestManager(this);
        userManager = new UserManager(this);

        clientManager.OnInit();
        uiManager.OnInit();
        requestManager.OnInit();
        userManager.OnInit();
    }

    /// <summary>
    /// 物体销毁时调用
    /// </summary>
    public void OnDestroy()
    {
        clientManager.OnDestroy();
        uiManager.OnDestroy();
        requestManager.OnDestroy();
        userManager.OnDestroy();
    }

    /// <summary>
    /// 通过clientManager向服务器发送请求
    /// </summary>
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientManager.SendRequest(requestCode, actionCode, data);
    }

    /// <summary>
    /// 对服务器发送回的数据进行响应
    /// </summary>
    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestManager.HandleResponse(actionCode, data);
    }

    /// <summary>
    /// 通过单例增加request字典
    /// </summary>
    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestManager.AddRequestDic(actionCode, baseRequest);
    }

    /// <summary>
    /// 通过单例移除request字典
    /// </summary>
    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }

    /// <summary>
    /// 用于设置用户的信息，在登录时
    /// </summary>
    /// <param name="userData"></param>
    public void SetUserData(UserData userData)
    {
        userManager.UserData = userData;
    }

    /// <summary>
    /// 获取用户的实例，方便设置
    /// </summary>
    /// <returns></returns>
    public UserData GetUserData()
    {
        return userManager.UserData;
    }

    /// <summary>
    /// 用于设置好友消息
    /// </summary>
    /// <param name="userData"></param>
    public void SetFriendUserData(UserData friendUserData)
    {
        userManager.FriendUserdata = friendUserData;
    }

    /// <summary>
    /// 用于获取好友消息
    /// </summary>
    /// <returns></returns>
    public UserData GetFriendUserData()
    {
        return userManager.FriendUserdata;
    }

    /// <summary>
    /// panel入栈
    /// </summary>
    public void PushPanel(UIPanelType uiPanelType)
    {
        uiManager.PushPanel(uiPanelType);
    }

    /// <summary>
    /// panel出栈
    /// </summary>
    public void PopPanel()
    {
        uiManager.PopPanel();
    }
}
