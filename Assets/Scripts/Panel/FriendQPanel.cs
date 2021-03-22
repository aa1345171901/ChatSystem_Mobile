﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendQPanel : BasePanel
{
    private GameObject messageImg;
    private GameObject friendImg;
    private GameObject friendQImg;

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton2");
        friendImg = GameObject.Find("DownColumn/friendButton2");
        friendQImg = GameObject.Find("DownColumn/friendQButton2");

        // 给物体添加事件
        messageImg.GetComponent<Button>().onClick.AddListener(OnClickMainBtn);
        friendImg.GetComponent<Button>().onClick.AddListener(OnClickFriendBtn);

    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendQImg);
    }

    /// <summary>
    /// panel继续
    /// </summary>
    public override void OnResume()
    {
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendQImg);
    }

    /// <summary>
    /// panel退出
    /// </summary>
    public override void OnExit()
    {
        HideAnimation(friendQImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        HideAnimation(friendQImg);
    }

    /// <summary>
    /// 联系人按钮点击
    /// </summary>
    private void OnClickMainBtn()
    {
        HideAnimation(messageImg);

        // 播放动画后push
        Invoke("PushMainPanel", 0.2f);
    }

    /// <summary>
    /// 动态按钮点击
    /// </summary>
    private void OnClickFriendBtn()
    {
        HideAnimation(friendImg);

        // 播放动画后push
        Invoke("PushFriendPanel", 0.2f);
    }

    /// <summary>
    /// 点击push FriendQPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushMainPanel()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.MainPanel);
    }

    /// <summary>
    /// 点击push friendPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushFriendPanel()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.FriendPanel);
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