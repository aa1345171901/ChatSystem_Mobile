using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private InputField nickNameIF;
    private InputField passwordIF;
    private InputField repeatPasswordIF;

    private Button registerButton;
    private Button backButton;

    private RegisterRequest registerRequest;

    private void Start()
    {
        // 获取组件
        registerRequest = GetComponent<RegisterRequest>();

        nickNameIF = transform.Find("NickNameIF").GetComponent<InputField>();
        passwordIF = transform.Find("PassWordIF").GetComponent<InputField>();
        repeatPasswordIF = transform.Find("RepeatPassWordIF").GetComponent<InputField>();

        // 给button添加事件
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        registerButton.onClick.AddListener(OnRegisterClick);

        backButton = transform.Find("back").GetComponent<Button>();
        backButton.onClick.AddListener(OnBackClick);
    }

    /// <summary>
    /// panel退出时的事件
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// panel进入时的事件
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBackClick()
    {
        uiMng.PopPanel();
    }

    /// <summary>
    /// 立即注册点击
    /// </summary>
    private void OnRegisterClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(repeatPasswordIF.text))
        {
            msg = "重复密码不能为空";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg = "密码不能为空";
        }
        if (string.IsNullOrWhiteSpace(nickNameIF.text))
        {
            msg = "昵称不能为空或不符合要求";
        }
        if (passwordIF.text != repeatPasswordIF.text)
        {
            msg = "密码和重复密码不一致";
        }
        try
        {
            // 验证成功就向服务器发送注册请求
            if (msg == "")
            {
                string nickName = nickNameIF.text.Trim();
                string password = passwordIF.text.Trim();
                string data = nickName + "," + password;
                registerRequest.SendRequest(data);
            }
            else
            {
                uiMng.ShowMessage(msg);
            }
        }
        catch (System.Exception e)
        {
            uiMng.ShowMessage("连接服务器失败:" + e.Message);
        }
    }

    /// <summary>
    /// 对注册的请求服务器的反馈做下一步操作
    /// </summary>
    public void OnResponseRegister(ReturnCode returnCode, int id)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync(string.Format("注册成功！\n您的账号是{0}", id));
            uiMng.PopPanelSync();
        }
        else
        {
            uiMng.ShowMessageSync("服务器没有响应，请稍后再试");
        }
    }
}
