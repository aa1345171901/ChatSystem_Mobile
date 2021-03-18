using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private InputField userIdIF;
    private InputField passWordIF;

    private Button loginButton;
    private Button registerButton;
    private Button findPassWord;

    private LoginRequest loginRequest;

    /// <summary>
    /// 获取子物体
    /// </summary>
    private void Start()
    {
        // 获取组件
        loginRequest = this.GetComponent<LoginRequest>();

        userIdIF = transform.Find("userIdIF").GetComponent<InputField>();
        passWordIF = transform.Find("PassWordIF").GetComponent<InputField>();

        // 给按钮添加事件
        loginButton = transform.Find("login").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginClick);

        registerButton = transform.Find("Register").GetComponent<Button>();
        registerButton.onClick.AddListener(OnRegisterClick);

        findPassWord = transform.Find("FindPassword").GetComponent<Button>();
        findPassWord.onClick.AddListener(OnFindPassWordClick);
    }

    /// <summary>
    /// 登录按钮点击
    /// </summary>
    private void OnLoginClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(userIdIF.text))
        {
            msg += "账号不能为空！";
        }
        if (string.IsNullOrEmpty(passWordIF.text))
        {
            msg += "密码不能为空！";
        }

        if (msg != "")
        {
            uiMng.ShowMessage(msg);
        }
        else
        {
            //发送用户名密码至服务器验证
            try
            {
                int id = int.Parse(userIdIF.text.Trim());
                string passWord = passWordIF.text.Trim();
                string data = id + "," + passWord;
                loginRequest.SendRequest(data);
            }
            catch (System.Exception)
            {
                uiMng.ShowMessage("连接服务器失败");
            }
        }
    }

    /// <summary>
    /// 登录时间响应
    /// </summary>
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("输入的账号错误或者密码错误，登录失败");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.MainPanel);
        }
    }

    /// <summary>
    /// 注册按钮点击
    /// </summary>
    private void OnRegisterClick()
    {
        uiMng.PushPanel(UIPanelType.RegisterPanel);
    }

    /// <summary>
    /// 找回密码按钮点击
    /// </summary>
    private void OnFindPassWordClick()
    {
        uiMng.ShowMessage("暂未开启，以后使用手机注册时可开启该功能");
    }
}
