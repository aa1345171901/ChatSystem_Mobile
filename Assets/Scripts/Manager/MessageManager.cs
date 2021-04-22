using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : BaseManager
{
    // 用于保存未读信息
    private Dictionary<int, string> dict = new Dictionary<int, string>();
    public Dictionary<int, string> GetUnreadMessage
    {
        get
        {
            return dict;
        }
        set
        {
            dict = value;
        }
    }

    private GetUnreadMessageRequest getMsgRequest;
    private float timer = 0;    //计时器，每1s发送读取消息请求

    public MessageManager(Facade facade, GetUnreadMessageRequest getMsgRequest):base(facade)
    {
        dict = null;
        this.getMsgRequest = getMsgRequest;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            getMsgRequest.SendMessage(facade.GetUserData().LoginId.ToString());
            timer = 0;
        }
    }
}
