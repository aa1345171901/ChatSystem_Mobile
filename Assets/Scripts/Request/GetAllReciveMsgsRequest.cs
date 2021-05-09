using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllReciveMsgsRequest : BaseRequest
{
    ChatPanel chatPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Message;
        actionCode = ActionCode.GetAllReceiveMsg;
        chatPanel = GetComponent<ChatPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            lock (chatPanel.getReceiveDict)
            {
                string dataStr = data.Substring(2);
                string[] dataStrs = dataStr.Split('-');
                string message;
                long ticks;
                for (int i = 0; i < dataStrs.Length - 2; i++)
                {
                    message = dataStrs[i].Split(',')[0];
                    ticks = long.Parse(dataStrs[i].Split(',')[1]);
                    chatPanel.getReceiveDict.Add((message, ticks));
                }
            }
        }
    }
}
