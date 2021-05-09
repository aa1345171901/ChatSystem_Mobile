using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatByReceiveRequest : BaseRequest
{
    private ChatPanel chatPanel;

    public override void Awake()
    {
        chatPanel = GetComponent<ChatPanel>();
        requestCode = RequestCode.Message;
        actionCode = ActionCode.ChatByReceive;
        base.Awake();
    }

    public void Init()
    {
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            lock (chatPanel)
            {
                int friendId = int.Parse(strs[1]);
                string message = strs[2];
                long ticks = long.Parse(strs[3]);
                chatPanel.OnResponseChatReceive(returnCode, message, ticks);
            }
        }
    }
}
