using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFriendListRequest : BaseRequest
{
    private FriendPanel friendPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Friend;
        actionCode = ActionCode.GetFriendList;
        friendPanel = GetComponent<FriendPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Fail)
        {
            friendPanel.OnResponseGetFriendList(returnCode, null);
        }
        else
        {
            string str = data.Substring(2);
            friendPanel.OnResponseGetFriendList(returnCode, DataHelper.StringToDic(str));
        }
    }
}
