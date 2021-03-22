using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanel : BasePanel
{
    public GameObject messageImg;
    private GameObject friendImg;
    private GameObject friendQImg;

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton1");
        friendImg = GameObject.Find("DownColumn/friendButton1");
        friendQImg = GameObject.Find("DownColumn/friendQButton1");

        // 给物体添加事件
        messageImg.GetComponent<Button>().onClick.AddListener(OnClickMainBtn);
        friendQImg.GetComponent<Button>().onClick.AddListener(OnClickFriendQBtn);

    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendImg);
    }

    /// <summary>
    /// panel继续
    /// </summary>
    public override void OnResume()
    {
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendImg);
    }

    /// <summary>
    /// panel退出
    /// </summary>
    public override void OnExit()
    {
        HideAnimation(friendImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        HideAnimation(friendImg);
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
    private void OnClickFriendQBtn()
    {
        HideAnimation(friendQImg);

        // 播放动画后push
        Invoke("PushFriendQPanel", 0.2f);
    }

    /// <summary>
    /// 点击push FriendQPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushMainPanel()
    {
        uiMng.PushPanel(UIPanelType.MainPanel);
    }

    /// <summary>
    /// 点击push friendPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushFriendQPanel()
    {
        uiMng.PushPanel(UIPanelType.FriendQPanel);
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
