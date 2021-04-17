using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSystemFaceRequest : BaseRequest
{
    private FacePanel facePanel;

    public override void Awake()
    {
        facePanel = GetComponent<FacePanel>();
        requestCode = RequestCode.User;
        actionCode = ActionCode.SetSystemFace;
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
            facePanel.OnResponseFaceChange(returnCode);
        }
        else
        {
            Facade.GetUserData().FaceId = int.Parse(strs[1]);
            facePanel.OnResponseFaceChange(returnCode);
        }
    }
}
