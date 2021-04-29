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
}
