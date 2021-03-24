using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 纪录用户的信息
/// </summary>
public class UserData
{
    /// <summary>
    /// 用于记录用户的id
    /// </summary>
    public int LoginId { get; set; }

    /// <summary>
    /// 用于记录用户数据的id，方便获取信息
    /// </summary>
    public int DataId { get; set; }

    /// <summary>
    /// 获取用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 设置的性别
    /// </summary>
    public string Sex { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 星座,对应数据库Star表
    /// </summary>
    public int StarId { get; set; }

    /// <summary>
    /// 血型,对应数据库BloodType表
    /// </summary>
    public int BloodTypeId { get; set; }

    /// <summary>
    /// 头像Id
    /// </summary>
    public int FaceId { get; set; }

    /// <summary>
    /// 添加好友策略Id
    /// </summary>
    public int FriendshipPolicyId { get; set; }

    /// <summary>
    /// 获取用户id
    /// </summary>
    public int GetId
    {
        get
        {
            return LoginId;
        }
    }
}
