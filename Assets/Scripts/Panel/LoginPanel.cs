using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private InputField userNameIF;
    private InputField passWordIF;
    private Button LoginButton;

    private LoginRequest loginRequest;

    /// <summary>
    /// 获取子物体
    /// </summary>
    private void Start()
    {
        loginRequest = this.GetComponent<LoginRequest>();

        userNameIF = transform.Find("UserNameIF").GetComponent<InputField>();
        passWordIF = transform.Find("PassWordIF").GetComponent<InputField>();

        LoginButton = transform.Find("login").GetComponent<Button>();
        LoginButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(userNameIF.text))
        {
            msg += "用户名不能为空！";
        }
        if (string.IsNullOrEmpty(passWordIF.text))
        {
            msg += "密码不能为空！";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
        }
        //发送用户名密码至服务器验证
        if (!string.IsNullOrEmpty(userNameIF.text))
        {
            loginRequest.SendRequest(userNameIF.text, passWordIF.text);
        }
    }

    public void OnLoginResponse(string data)
    {
        uiMng.ShowMessageSync(data);
    }
}
