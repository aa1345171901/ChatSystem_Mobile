using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFriendRequest : BaseRequest
{
    private AddFriendPanel addFriendPanel;

    public override void Awake()
    {
        addFriendPanel = GetComponent<AddFriendPanel>();
        requestCode = RequestCode.Friend;
        actionCode = ActionCode.AddFriend;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Fail)
        {
            addFriendPanel.OnResponseAddFriend(returnCode, strs[1]);
        }
        else
        {
            addFriendPanel.OnResponseAddFriend(returnCode, "");
        }
    }
}
