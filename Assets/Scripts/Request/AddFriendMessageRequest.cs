using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFriendMessageRequest : BaseRequest
{
    private SystemPanel systemPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Message;
        actionCode = ActionCode.AddFriendMessageRequest;
        systemPanel = GetComponent<SystemPanel>();
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
            int fromUserId = int.Parse(strs[1]);
            systemPanel.OnResponseGetRequest(returnCode);
        }
        else
        {
            int fromUserId = int.Parse(strs[1]);
            string nickName = strs[2];
            systemPanel.friendDict[fromUserId] = (nickName, systemPanel.friendDict[fromUserId].Item2);
            systemPanel.isHaveUnread = true;
        }
    }
}
