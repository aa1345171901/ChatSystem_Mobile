using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendQPanel : BasePanel
{
    private GameObject messageImg;
    private GameObject friendImg;
    private GameObject friendQImg;

    private Image face;

    private Button friendNewsBtn;

    private RectTransform content;

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton2");
        friendImg = GameObject.Find("DownColumn/friendButton2");
        friendQImg = GameObject.Find("DownColumn/friendQButton2");

        friendNewsBtn = transform.Find("Scroll View/Viewport/Content/FriendNews").GetComponent<Button>();
        friendNewsBtn.onClick.AddListener(OnClickFriendNews);

        // 给物体添加事件
        messageImg.GetComponent<Button>().onClick.AddListener(OnClickMainBtn);
        friendImg.GetComponent<Button>().onClick.AddListener(OnClickFriendBtn);

        // 获取组件
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        face = transform.Find("TopColumn/face").GetComponent<Image>();
    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendQImg);

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

        // 给头像赋值
        string facePath = "FaceImage/" + Facade.GetUserData().FaceId;
        Sprite faceImg = Resources.Load<Sprite>(facePath);
        face.sprite = faceImg;
    }

    /// <summary>
    /// panel退出
    /// </summary>
    public override void OnExit()
    {
        gameObject.SetActive(false);
        HideAnimation(friendQImg);
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
    private void OnClickMainBtn()
    {
        HideAnimation(messageImg);

        // 播放动画后push
        Invoke("PushMainPanel", 0.1f);
    }

    /// <summary>
    /// 动态按钮点击
    /// </summary>
    private void OnClickFriendBtn()
    {
        HideAnimation(friendImg);

        // 播放动画后push
        Invoke("PushFriendPanel", 0.1f);
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
    /// 好友动态点击
    /// </summary>
    private void OnClickFriendNews()
    {
        uiMng.ShowMessage("暂未开发该功能,,ԾㅂԾ,,");
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
            offset = offset > 720 ? 720 : offset;
            // 当panel在中间才能向右滑
            if (this.transform.localPosition.x < 720)
            {
                this.transform.localPosition = new Vector3(offset, 0, 0);
            }
        }
        else
        {
            offset = offset < -720 ? -720 : offset;
            // 当panel在右边才能向左滑
            if (this.transform.localPosition.x > 0)
            {
                this.transform.localPosition = new Vector3(720 + offset, 0, 0);
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
            // 当panel在右边才能向左滑
            if (this.transform.localPosition.x > 0)
            {
                Tween t = this.transform.DOLocalMoveX(720, 0.2f);
            }
        }
        else
        {
            this.transform.DOLocalMoveX(0, 0.2f);
        }
    }
}
