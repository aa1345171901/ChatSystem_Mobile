using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : BaseManager
{
    // 用于保存未读信息
    private Dictionary<int, string> dict = new Dictionary<int, string>();
    public Dictionary<int, string> GetUnreadMessage
    {
        set
        {
            dict = value;
        }
    }

    public Dictionary<int, (int, string, string, string)> UserFriendMsgDic { get; } // 消息的发起者与发消息的好友的头像Id
    public Dictionary<int, int> SystemMsgDic { get; }   // 系统消息好友的头像Id

    private GetUnreadMessageRequest getMsgRequest;
    private float timer = 0;    //计时器，每1s发送读取消息请求

    public MessageManager(Facade facade, GetUnreadMessageRequest getMsgRequest):base(facade)
    {
        dict = null;
        UserFriendMsgDic = new Dictionary<int, (int, string, string, string)>();
        SystemMsgDic = new Dictionary<int, int>();
        this.getMsgRequest = getMsgRequest;
        //getMsgRequest.SendRequest(facade.GetUserData().LoginId.ToString());
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
        {
            getMsgRequest.SendRequest(facade.GetUserData().LoginId.ToString());
            timer = 0;
        }

        if (dict != null)
        {
            SplitSystemAndFriendMsg();
            dict = null;
        }
    }

    private void SplitSystemAndFriendMsg()
    {
        if (dict != null)
        {
            foreach (var item in dict)
            {
                int messageTypeId = -1;
                int messageState = -1;
                string[] strs = item.Value.Split(',');
                messageTypeId = int.Parse(strs[1]);
                messageState = int.Parse(strs[2]);
                if (int.Parse(strs[3]) == 0)
                {
                    if (messageTypeId == 1)
                    {
                        // 好友消息，提示音
                    }

                    if (messageTypeId == 2)
                    {
                        // 系统消息，提示音
                    }
                }

                // 判断消息类型，如果是添加好友消息
                if (messageTypeId == 2 && messageState == 0)
                {
                    if (!SystemMsgDic.ContainsKey(int.Parse(strs[0])))
                    {
                        SystemMsgDic.Add(int.Parse(strs[0]), int.Parse(strs[4]));
                    }
                }

                // 如果是聊天消息
                if (messageTypeId == 1 && messageState == 0)
                {
                    try
                    {
                        if (!UserFriendMsgDic.ContainsKey(int.Parse(strs[0])))
                        {
                            UserFriendMsgDic.Add(int.Parse(strs[0]), (int.Parse(strs[4]), strs[5], strs[6], strs[7]));   // 设置发消息的好友的头像索引
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
            }
        }
    }

    public void ClearSystem()
    {
        SystemMsgDic.Clear();
    }

    public void ClearFriend()
    {
        UserFriendMsgDic.Clear();
    }
}
