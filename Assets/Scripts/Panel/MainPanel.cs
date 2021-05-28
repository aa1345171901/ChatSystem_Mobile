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

    private ScreenSwipe screenSwipe;

    private string ChatData;
    private GameObject freindPanelHave;  // 是否有好友请求
    private Dictionary<int, (int, string, string, string)> chatDic = new Dictionary<int, (int, string, string, string)>();  // 好友消息
    private List<GameObject> chatItems = new List<GameObject>();
    public int getCount = 0;  // 获取的消息数量

    private void Awake()
    {
        // 获取游戏物体
        messageImg = GameObject.Find("DownColumn/messageButton");
        friendImg = GameObject.Find("DownColumn/friendButton");
        friendQImg = GameObject.Find("DownColumn/friendQButton");
        freindPanelHave = GameObject.Find("DownColumn/friendButton/have");

        // 给物体添加事件
        friendImg.GetComponent<Button>().onClick.AddListener(OnClickFriendBtn);
        friendQImg.GetComponent<Button>().onClick.AddListener(OnClickFriendQBtn);
        freindPanelHave.SetActive(false);

        // 寻找组件
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        nickName = transform.Find("TopColumn/NickName").GetComponent<Text>();
        face = transform.Find("TopColumn/face").GetComponent<Image>();

        screenSwipe = GetComponent<ScreenSwipe>();
    }

    private void Start()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Item/InputField"));
        go.transform.SetParent(content, false);

        // 设置layout空隙
        GameObject space = Instantiate(Resources.Load<GameObject>("Item/Spacing"));
        space.transform.SetParent(content, false);

        // 获取本地的好友消息漫游
        string chatsStr = PlayerPrefs.GetString(Facade.GetUserData().LoginId + "chats");
        if (!string.IsNullOrEmpty(chatsStr))
        {
            string[] strs = chatsStr.Split('-');
            for (int i = 0; i < strs.Length - 1; i++)
            {
                int id = int.Parse(strs[i].Split(',')[0]);
                int faceid = int.Parse(strs[i].Split(',')[1]);
                string nickName = strs[i].Split(',')[2];
                string message = strs[i].Split(',')[3];
                string date = strs[i].Split(',')[4];
                chatDic.Add(id, (faceid, nickName, message, date));
            }
            SetChatItem();
        }
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

        freindPanelHave.SetActive(false);

        // 不在start赋值，可能更改个人消息
        // 给nickName赋值
        nickName.text = Facade.GetUserData().NickName;

        // 给头像赋值
        string facePath = "FaceImage/" + Facade.GetUserData().FaceId;
        Sprite faceImg = Resources.Load<Sprite>(facePath);
        face.sprite = faceImg;
    }

    private void Update()
    {
        if (Facade.GetUnreadFriendMsg().Count > 0)
        {
            Dictionary<int, (int, string, string, string)> dict = Facade.GetUnreadFriendMsg();
            foreach (var item in dict)
            {
                //GetReceive(item.Key);
                if (chatDic.ContainsKey(item.Key))
                {
                    chatDic[item.Key] = item.Value;
                }
                else
                {
                    chatDic.Add(item.Key, item.Value);
                }
            }
            SetChatItem();
            PlayerPrefs.SetString(Facade.GetUserData().LoginId + "chats", ChatData);
            Facade.ClearFriend();
        }

        if (Facade.GetUnreadSystemMsg().Count > 0)
        {
            freindPanelHave.SetActive(true);
        }
        else
        {
            freindPanelHave.SetActive(false);
        }
    }

    /// <summary>
    /// panel继续
    /// </summary>
    public override void OnResume()
    {
        gameObject.SetActive(true);
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
        Invoke("SetActive", 0.5f);
        HideAnimation(messageImg);
    }

    /// <summary>
    /// panel暂停
    /// </summary>
    public override void OnPause()
    {
        Invoke("SetActive", 0.5f);
        screenSwipe.enabled = false;
    }

    private void SetActive()
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
        SetActive();
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.FriendPanel);
    }

    /// <summary>
    /// 点击push FriendQPanel
    /// </summary>
    /// <param name="uIPanelType"></param>
    private void PushFriendQPanel()
    {
        SetActive();
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

    public void AddDict(int id, string nickName, string message, long ticks)
    {
        DateTime sendTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        sendTime = sendTime.AddMilliseconds(ticks);
        chatDic[id] = (chatDic[id].Item1,nickName, message, sendTime.ToLongTimeString());
    }

    /// <summary>
    /// 设置好友消息列表
    /// </summary>
    private void SetChatItem()
    {
        // 设置layout大小
        Vector2 size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, 80 + 120 * (chatDic.Count));

        ChatData = "";

        int[] keys = new int[chatDic.Count];
        int j = 0;
        foreach (int key in chatDic.Keys) keys[j++] = key;

        for (int i = 0; i < chatDic.Count; i++)
        {
            chatDic.TryGetValue(keys[i], out (int, string, string, string) item);
            ChatData += keys[i] + "," + item.Item1 + "," + item.Item2 + "," + item.Item3 + "," + item.Item4 + "-";
            // friendGOs里有就不用生成
            GameObject go;
            if (chatItems.Count > i)
            {
                go = chatItems[i];
            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>("Item/ChatItem"));
                chatItems.Add(go);
            }

            string nickName = item.Item2;
            int faceId = item.Item1;

            // 设置子物体属性
            go.transform.Find("NickName").GetComponent<Text>().text = nickName;
            go.transform.Find("ChatRecord").GetComponent<Text>().text = item.Item3;
            DateTime sendTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            sendTime = sendTime.AddMilliseconds(long.Parse(item.Item4));
            go.transform.Find("Date").GetComponent<Text>().text = sendTime.ToLongTimeString();
            go.name = keys[i].ToString();

            string facePath = "FaceImage/" + faceId;
            Sprite face = Resources.Load<Sprite>(facePath);
            go.transform.Find("FaceMask/Image").GetComponent<Image>().sprite = face;

            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(OnClickChatItem);
            // 设置父物体
            go.transform.SetParent(content, false);

            if (Facade.GetUnreadFriendMsg().ContainsKey(keys[i]))
            {
                go.transform.Find("Have").gameObject.SetActive(true);
            }
        }
    }

    private void OnClickChatItem()
    {
        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int friendId = int.Parse(button.name);

        uiMng.PopPanel();
        button.transform.Find("Have").gameObject.SetActive(false);
        ChatPanel chatPanel = uiMng.PushPanel(UIPanelType.ChatPanel) as ChatPanel;
        int faceId = 1;
        int.TryParse(button.transform.Find("FaceMask/Image").GetComponent<Image>().sprite.name.Trim(), out faceId);
        chatPanel.SetDetail(friendId, nickName.text, faceId);
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
            offset = offset > 1080 ? 1080 : offset;
            // 当panel在中间才能向右滑
            if (this.transform.localPosition.x < 1080)
            {
                this.transform.localPosition = new Vector3(offset, 0, 0);
            }
        }
        else
        {
            offset = offset < -1080 ? -1080 : offset;
            // 当panel在右边才能向左滑
            if (this.transform.localPosition.x > 0)
            {
                this.transform.localPosition = new Vector3(1080 + offset, 0, 0);
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
                Tween t = this.transform.DOLocalMoveX(1080, 0.2f);
            }
            // t.OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            this.transform.DOLocalMoveX(0, 0.2f);
        }
    }
}
