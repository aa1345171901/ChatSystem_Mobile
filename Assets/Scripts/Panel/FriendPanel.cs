using Common;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanel : BasePanel
{
    private GameObject messageImg;
    private GameObject friendImg;
    private GameObject friendQImg;

    private Image face;

    private RectTransform content;
    private Transform reflash;   // 用于获取更新的panel位置，下拉更新

    private GetFriendListRequest getFriendListRequest;   // 获取好友列表的request

    private Dictionary<int, (string, int)> FriendDic = null;   // 用于设置好友列表 <账号，(昵称，头像ID)>
    private List<GameObject> friendGOs = new List<GameObject>();   // 好友列表Item列表，刷新时先删除

    private bool isSF = false;    // 判断是否释放，不能发送请求太快 

    private Button addFriendImg;   // 右上角的好友添加按钮
    private Button addFriendText;   // 列表的新朋友按钮

    private ScreenSwipe screenSwipe;   // 控制滑动panel失效

    private GameObject goNewFriendRightTop; // 新朋友，用于设置右上红点
    private GameObject mainHave;    // 用于设置消息panel是否

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton1");
        friendImg = GameObject.Find("DownColumn/friendButton1");
        friendQImg = GameObject.Find("DownColumn/friendQButton1");
        mainHave = GameObject.Find("DownColumn/messageButton1/have");

        // 获取组件
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
        reflash = transform.Find("Scroll View/Viewport/Reflash").GetComponent<Transform>();

        face = transform.Find("TopColumn/face").GetComponent<Image>();

        addFriendImg = transform.Find("TopColumn/AddFriendBtn").GetComponent<Button>();

        screenSwipe = GetComponent<ScreenSwipe>();

        // 给物体添加事件
        messageImg.GetComponent<Button>().onClick.AddListener(OnClickMainBtn);
        friendQImg.GetComponent<Button>().onClick.AddListener(OnClickFriendQBtn);

        getFriendListRequest = GetComponent<GetFriendListRequest>();

        addFriendImg.onClick.AddListener(AddFriendClick);

        mainHave.SetActive(false);
    }

    /// <summary>
    /// Start周期发送获取好友列表的请求,添加搜索
    /// </summary>
    private void Start()
    {
        GameObject goSearch = Instantiate(Resources.Load<GameObject>("Item/InputField"));
        goSearch.transform.SetParent(content,false);

        // 设置layout空隙
        GameObject space = Instantiate(Resources.Load<GameObject>("Item/Spacing"));
        space.transform.SetParent(content, false);

        GameObject goNewFriend = Instantiate(Resources.Load<GameObject>("Item/NewFriend"));
        goNewFriend.transform.SetParent(content, false);
        goNewFriendRightTop = goNewFriend.transform.Find("Have").gameObject;

        // 给按钮添加事件
        addFriendText = goNewFriend.GetComponent<Button>();
        addFriendText.onClick.AddListener(SystemFriend);

        // 设置layout空隙
        space = Instantiate(Resources.Load<GameObject>("Item/Spacing"));
        space.transform.SetParent(content, false);

        // 获取本地的好友列表
        string friendsStr = PlayerPrefs.GetString(Facade.GetUserData().LoginId + ",friends");
        if (!string.IsNullOrEmpty(friendsStr))
        {
            FriendDic = DataHelper.StringToDic(friendsStr);
        }
    }

    /// <summary>
    /// 每帧调用用于异步
    /// </summary>
    private void Update()
    {
        // 释放立即更新好友列表
        if (content.localPosition.y < -50)
        {
            isSF = true;
        }
        if (content.localPosition.y >= -5 && isSF)
        {
            isSF = false;
            GetFriendList();
            DateTime ReTime = DateTime.Now;
            string ReTimeStr = "    " + ReTime.Hour + " : " + ReTime.Minute + "\n" + ReTime.Year + " / " + ReTime.Month + " / " + ReTime.Day;
            PlayerPrefs.SetString(Facade.GetUserData().LoginId + ",ReTime", ReTimeStr);   // 通过id区分不同的账号刷新时间
        }

        // 从服务器获取的好友列表信息
        if (FriendDic != null)
        {
            SetFriendItem();

            // 将friends保存到本地，不刷新时就可以不访问服务器
            PlayerPrefs.SetString(Facade.GetUserData().LoginId + ",friends", DataHelper.DicToString(FriendDic));
            FriendDic = null;
        }

        // 如果有未读的系统消息，显示右上红点
        if (Facade.GetUnreadSystemMsg().Count > 0)
        {
            goNewFriendRightTop.SetActive(true);
        }
        else
        {
            goNewFriendRightTop.SetActive(false);
        }

        if (Facade.GetUnreadFriendMsg().Count > 0)
        {
            mainHave.SetActive(true);
        }
        else
        {
            mainHave.SetActive(false);
        }
    }

    /// <summary>
    /// panel进入
    /// </summary>
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        messageImg.transform.localScale = new Vector3(1, 1, 1);
        friendQImg.transform.localScale = new Vector3(1, 1, 1);
        EnterAnimation(friendImg);
        mainHave.SetActive(false);

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
        GetFriendList();
        screenSwipe.enabled = true;
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
        HideAnimation(friendImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        Invoke("SetActive",0.5f);
        screenSwipe.enabled = false;
    }

    private void SetActive()
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
    private void OnClickFriendQBtn()
    {
        HideAnimation(friendQImg);

        // 播放动画后push
        Invoke("PushFriendQPanel", 0.1f);
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
    /// 获取好友列表
    /// </summary>
    private void GetFriendList()
    {
        try
        {
            getFriendListRequest.SendRequest(Facade.GetUserData().GetId.ToString());
        }
        catch (System.Exception e)
        {
            uiMng.ShowMessage("暂时无法更新好友列表，请检查您的网络");
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 获取好友列表反馈
    /// </summary>
    public void OnResponseGetFriendList(ReturnCode returnCode, Dictionary<int, (string, int)> friends)
    {
        if (returnCode == ReturnCode.Success)
        {
            FriendDic = friends;
        }
        else
        {
            uiMng.ShowMessageSync("无法获取好友列表,,ԾㅂԾ,,");
        }
    }

    /// <summary>
    /// 设置好友的Item
    /// </summary>
    private void SetFriendItem()
    {
        // 设置layout大小
        Vector2 size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, 25 + 40 * (FriendDic.Count + 1));

        int index = 0;
        foreach (var item in FriendDic)
        {
            // friendGOs里有就不用生成
            GameObject go;
            if (index < friendGOs.Count)
            {
                go = friendGOs[index++];
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>("Item/Friend"));
                friendGOs.Add(go);

                // 不自增只能生成一个物体
                index++;
            }
            string nickName = item.Value.Item1;
            int faceId = item.Value.Item2;

            // 设置子物体属性
            go.GetComponentInChildren<Text>().text = nickName;
            go.name = item.Key.ToString();

            string facePath = "FaceImage/" + faceId;
            Sprite face = Resources.Load<Sprite>(facePath);
            go.transform.Find("FaceMask/Image").GetComponent<Image>().sprite = face;

            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(OnFriendItemClick);

            // 设置父物体
            go.transform.SetParent(content, false);
        }
        // 更新后好友数量少了，将后面的设为不可见
        if (index < friendGOs.Count)
        {
            for(int i = index;i < friendGOs.Count; i++)
            {
                friendGOs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 好友列表子物体点击
    /// </summary>
    private void OnFriendItemClick()
    {
        FriendDetailPanel friendDetailPanel = uiMng.PushPanel(UIPanelType.FriendDetailPanel) as FriendDetailPanel;
        screenSwipe.OnInit();
        friendDetailPanel.btnText.text = "发消息";
        friendDetailPanel.OnSetDelete();

        GetFriendDetailRequest getDetail = friendDetailPanel.GetComponent<GetFriendDetailRequest>();

        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int friendId = int.Parse(button.name);

        friendDetailPanel.idText.text = "账号 :" + friendId.ToString();

        // 获取详细信息请求
        getDetail.SendRequest(friendId.ToString());
    }

    /// <summary>
    /// 右上角添加好友按钮点击
    /// </summary>
    private void AddFriendClick()
    {
        uiMng.PushPanel(UIPanelType.AddFriendPanel);
        screenSwipe.OnInit();
    }

    /// <summary>
    /// 点击新朋友
    /// </summary>
    private void SystemFriend()
    {
        uiMng.PushPanel(UIPanelType.SystemPanel);
        screenSwipe.OnInit();
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
            // 当panel在中间才能向右滑
            if (this.transform.localPosition.x < 300)
            {
                this.transform.localPosition = new Vector3(offset, 0, 0);
            }
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
            // 当panel在右边才能向左滑
            if (this.transform.localPosition.x > 0)
            {
                Tween t = this.transform.DOLocalMoveX(300, 0.2f);
            }
        }
        else
        {
            this.transform.DOLocalMoveX(0, 0.2f);
        }
    }
}
