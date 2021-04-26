using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFriendRequest : BaseRequest
{
    private FriendDetailPanel friendDetailPanel;

    public override void Awake()
    {
        friendDetailPanel = GetComponent<FriendDetailPanel>();
        requestCode = RequestCode.Friend;
        actionCode = ActionCode.DeleteFriend;
        base.Awake();
    }

    /// <summary>
    /// 对服务器传递的消息响应
    /// </summary>
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        friendDetailPanel.OnResponseDelete(returnCode);
    }
}
