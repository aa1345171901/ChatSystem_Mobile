using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private GameObject messageImg;
    private GameObject friendImg;
    private GameObject friendQImg;

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton");
        friendImg = GameObject.Find("DownColumn/friendButton");
        friendQImg = GameObject.Find("DownColumn/friendQButton");

        // 给物体添加事件
        friendImg.GetComponent<Button>().onClick.AddListener(OnClickFriendBtn);
        friendQImg.GetComponent<Button>().onClick.AddListener(OnClickFriendQBtn);

    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(messageImg);
    }

    /// <summary>
    /// panel继续
    /// </summary>
    public override void OnResume()
    {
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(messageImg);
    }

    /// <summary>
    /// panel退出
    /// </summary>
    public override void OnExit()
    {
        HideAnimation(messageImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        HideAnimation(messageImg);
    }

    /// <summary>
    /// 联系人按钮点击
    /// </summary>
    private void OnClickFriendBtn()
    {
        HideAnimation(friendImg);
    }

    /// <summary>
    /// 动态按钮点击
    /// </summary>
    private void OnClickFriendQBtn()
    {
        HideAnimation(friendQImg);
    }

    /// <summary>
    /// 图片渐显动画
    /// </summary>
    public void EnterAnimation(GameObject go)
    {
        go.transform.localScale = new Vector3(0, 0, 0);
        go.transform.DOScale(1, 0.5f);
    }

    /// <summary>
    /// 图片渐显=隐动画
    /// </summary>
    public void HideAnimation(GameObject go)
    {
        Tween tween = go.transform.DOScale(0, 0.2f);
    }
}
