using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyRequest : BaseRequest
{
    private ModifyDetailPanel modifyDetailPanel;

    public override void Awake()
    {
        modifyDetailPanel = GetComponent<ModifyDetailPanel>();
        requestCode = RequestCode.User;
        actionCode = ActionCode.Modify;
        base.Awake();
    }

    /// <summary>
    /// 对服务器传递的消息响应
    /// </summary>
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Fail)
        {
            modifyDetailPanel.OnResponseModifyDetail(returnCode);
        }
        else
        {
            modifyDetailPanel.OnResponseModifyDetail(returnCode);
        }
    }
}
