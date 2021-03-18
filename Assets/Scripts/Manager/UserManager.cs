using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : BaseManager
{
    // 用于保存用户数据
    public UserData userdata { set; get; }

    public UserManager(Facade facade) : base(facade)
    {
    }
}
