using Common;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SearchFriendRequest : BaseRequest
{
    private AddFriendPanel addFriendPanel;

    public override void Awake()
    {
        actionCode = ActionCode.SearchFriend;
        requestCode = RequestCode.Friend;
        addFriendPanel = GetComponent<AddFriendPanel>();
        base.Awake();
    }

    /// <summary>
    /// 搜索好友响应，设置搜到的好友列表
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Fail)
        {
            addFriendPanel.OnResponseSearch(returnCode, null);
        }
        else
        {
            DataSet dataSet = DataHelper.DataSetFromString(strs[1]);
            addFriendPanel.OnResponseSearch(returnCode, DataHelper.DataSetValueToList(dataSet));
        }
    }
}
