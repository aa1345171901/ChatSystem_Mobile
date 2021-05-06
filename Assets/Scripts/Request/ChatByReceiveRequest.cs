using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatByReceiveRequest : BaseRequest
{
    private MainPanel mainPanel;

    public override void Awake()
    {
        mainPanel = GetComponent<MainPanel>();
        requestCode = RequestCode.Message;
        actionCode = ActionCode.ChatByReceive;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            lock (mainPanel)
            {
                int friendId = int.Parse(strs[1]);
                string message = strs[2];
                long ticks = long.Parse(strs[3]);
                string nickName = strs[4];
                mainPanel.getCount++;
                mainPanel.AddDict(friendId, nickName, message, ticks);
            }
        }
    }
}
