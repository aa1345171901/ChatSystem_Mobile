using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : BaseManager
{
    // 用于保存用户数据
    private UserData userdata;

    public UserData UserData
    {
        get
        {
            return userdata;
        }
        set
        {
            userdata = value;
        }
    }

    // 用于保存打开好友数据
    private UserData friendUserdata;

    public UserData FriendUserdata
    {
        get
        {
            return friendUserdata;
        }
        set
        {
            userdata = value;
        }
    }

    public UserManager(Facade facade) : base(facade)
    {
        userdata = new UserData();
        friendUserdata = new UserData();
    }
}
