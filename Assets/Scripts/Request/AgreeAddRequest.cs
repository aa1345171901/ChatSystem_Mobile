using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgreeAddRequest : BaseRequest
{
    private SystemPanel systemPanel; 

    public override void Awake()
    {
        requestCode = RequestCode.Friend;
        actionCode = ActionCode.AgreeAddFriend;
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
            int friendId = int.Parse(strs[1]);
            systemPanel.OnResponseAgree(returnCode, "添加失败，服务器出错");
            systemPanel.isAdd = false;
        }
        else
        {
            int friendId = int.Parse(strs[1]);
            systemPanel.OnResponseAgree(returnCode, "添加成功,请刷新显示");
            systemPanel.isAdd = true;
        }
    }
}
