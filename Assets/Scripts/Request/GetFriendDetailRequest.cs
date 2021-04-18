using Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GetFriendDetailRequest : BaseRequest
{
    private FriendDetailPanel friendDetailPanel;

    public override void Awake()
    {
        friendDetailPanel = GetComponent<FriendDetailPanel>();
        requestCode = RequestCode.Friend;
        actionCode = ActionCode.GetFriendDetail;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);

        friendDetailPanel.OnGetDetailResponse(returnCode);
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
            Facade.SetFriendUserData(userData);
            friendDetailPanel.isGet = true;
        }
    }
}
