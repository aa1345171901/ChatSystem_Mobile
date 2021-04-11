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

    private RectTransform content;

    private Text nickName;
    private Image face;

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton");
        friendImg = GameObject.Find("DownColumn/friendButton");
        friendQImg = GameObject.Find("DownColumn/friendQButton");

        // 给物体添加事件
        friendImg.GetComponent<Button>().onClick.AddListener(OnClickFriendBtn);
        friendQImg.GetComponent<Button>().onClick.AddListener(OnClickFriendQBtn);

        // 寻找组件
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        nickName = transform.Find("TopColumn/NickName").GetComponent<Text>();
        face = transform.Find("TopColumn/face").GetComponent<Image>();
    }

    private void Start()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Item/InputField"));
        go.transform.SetParent(content, false);

        // 设置layout空隙
        GameObject space = Instantiate(Resources.Load<GameObject>("Item/Spacing"));
        space.transform.SetParent(content, false);
    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(messageImg);

        // 不在start赋值，可能更改个人消息
        // 给nickName赋值
        nickName.text = Facade.GetUserData().NickName;

        // 给头像赋值
        string facePath = "FaceImage/" + Facade.GetUserData().FaceId;
        Sprite faceImg = Resources.Load<Sprite>(facePath);
        face.sprite = faceImg;
    }

    /// <summary>
    /// panel继续
    /// </summary>
    public override void OnResume()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// panel退出
    /// </summary>
    public override void OnExit()
    {
        gameObject.SetActive(false);
        HideAnimation(messageImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 联系人按钮点击
    /// </summary>
    private void OnClickFriendBtn()
    {
        HideAnimation(friendImg);

        // 播放动画后push
        Invoke("PushFriendPanel", 0.1f);
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
    /// 点击push friendPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushFriendPanel()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.FriendPanel);
    }

    /// <summary>
    /// 点击push FriendQPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushFriendQPanel()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.FriendQPanel);
    }

    /// <summary>
    /// 图片渐显动画
    /// </summary>
    public void EnterAnimation(GameObject go)
    {
        go.transform.localScale = new Vector3(0, 0, 0);
        go.transform.DOScale(1, 0.2f);
    }

    /// <summary>
    /// 图片渐显=隐动画
    /// </summary>
    public void HideAnimation(GameObject go)
    {
        Tween tween = go.transform.DOScale(0, 0.1f);
    }

    /// <summary>
    /// 跟随滑动
    /// </summary>
    /// <param name="offset"></param>
    public void OnFingerMove(int offset)
    {
        // 向右滑
        if (offset > 0)
        {
            offset = offset > 300 ? 300 : offset;
            this.transform.localPosition = new Vector3(offset, 0, 0);
        }
        else
        {
            offset = offset < -300 ? -300 : offset;
            // 当panel在右边才能向左滑
            if (this.transform.localPosition.x > 0)
            {
                this.transform.localPosition = new Vector3(300 + offset, 0, 0);
            }
        }
    }

    /// <summary>
    /// 手指右滑事件,判断是否满足右滑距离
    /// </summary>
    public void OnFingerAction(bool isMove)
    {
        if (isMove)
        {
            Tween t = this.transform.DOLocalMoveX(300, 0.2f);
            // t.OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            this.transform.DOLocalMoveX(0, 0.2f);
        }
    }
}
