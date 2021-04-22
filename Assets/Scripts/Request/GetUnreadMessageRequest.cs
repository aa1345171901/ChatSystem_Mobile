using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUnreadMessageRequest : BaseRequest
{

    public override void Awake()
    {
        requestCode = RequestCode.Message;
        actionCode = ActionCode.GetUnreadMessage;
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
            Facade.SetUnreadMsg(null);
        }
        else
        {
            string strList = data.Substring(2);
            string[] listString = strList.Split('-');
            Dictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < listString.Length - 1; i++)
            {
                string[] strs1 = listString[i].Split('_');
                if (!dict.ContainsKey(int.Parse(strs1[0])))
                {
                    dict.Add(int.Parse(strs1[0]), strs1[1]);
                }
            }

            Facade.SetUnreadMsg(dict);
        }
    }
}
