using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    private RegisterPanel registerPanel;

    // Awake周期设置各项属性
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            int id = int.Parse(strs[1]);
            int dataId = int.Parse(strs[2]);
            Facade.GetUserData().LoginId = id;
            Facade.GetUserData().DataId = dataId;

            registerPanel.OnResponseRegister(returnCode, id);
        }
        else
        {
            registerPanel.OnResponseRegister(returnCode, 0);
        }
    }
}
