using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendByChatRequest : BaseRequest
{
    private ChatPanel chatPanel;

    public override void Awake()
    {
        chatPanel = GetComponent<ChatPanel>();
        requestCode = RequestCode.Message;
        actionCode = ActionCode.SendByChat;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        chatPanel.OnResponseChatSend(returnCode);
    }
}
