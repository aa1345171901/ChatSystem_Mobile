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
        base.OnResponse(data);
    }

    public override void SendRequest(string data)
    {
        base.SendRequest(data);
    }
}
