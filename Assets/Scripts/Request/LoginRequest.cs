using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;

    // 初始化设置各属性
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = this.GetComponent<LoginPanel>();
        base.Awake();
    }

    // 登录事件
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);

        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {
            UserData userData = new UserData();
            userData.LoginId = int.Parse(strs[1]);
            userData.DataId = int.Parse(strs[2]);
            userData.NickName = strs[3];
            userData.Sex = strs[4];
            userData.Age = int.Parse(strs[5]);
            userData.Name = strs[6];
            userData.StarId = int.Parse(strs[7]);
            userData.BloodTypeId = int.Parse(strs[8]);
            userData.FaceId = int.Parse(strs[9]);
            Facade.SetUserData(userData);
        }
    }
}
