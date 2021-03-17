using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = this.GetComponent<LoginPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        loginPanel.OnLoginResponse(data);
    }

    public void SendRequest(string id, string password)
    {
        string data = id + "," + password;
        SendRequest(data);
    }
}
