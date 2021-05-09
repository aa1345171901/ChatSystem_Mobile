using Common;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    private Button backBtn;
    private Button sendBtn;
    private Text nickName;
    private InputField input;

    private RectTransform content;
    private List<GameObject> chatItems = new List<GameObject>();
    private List<string> messages = new List<string>();
    public List<(string, long)> getReceiveDict = new List<(string, long)>();
    private Scrollbar scrollbar;

    private int friendId;
    private int faceId;
    private bool isSend = false;

    private string message = null; // 接收一条消息
    private long ticks;

    private ChatByReceiveRequest chatReceiveRequest;
    private SendByChatRequest chatSendRequest;
    private GetAllReciveMsgsRequest getAllReciveMsgs;

    private float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        // 获取组件
        backBtn = this.transform.Find("ChatTopColumn/back").GetComponent<Button>();
        nickName = this.transform.Find("ChatTopColumn/nickName").GetComponent<Text>();
        sendBtn = this.transform.Find("DownColumn/SendBtn").GetComponent<Button>();
        input = this.transform.Find("DownColumn/InputField").GetComponent<InputField>();

        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        chatReceiveRequest = GetComponent<ChatByReceiveRequest>();
        chatSendRequest = GetComponent<SendByChatRequest>();
        getAllReciveMsgs = GetComponent<GetAllReciveMsgsRequest>();

        scrollbar = transform.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();

        // 设置事件
        backBtn.onClick.AddListener(BackBtnClick);
        sendBtn.onClick.AddListener(SendBtnClick);
    }

    private void Start()
    {
        //Invoke("GetReceive", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (message != null && IsEnter)
        {
            SetChatItem(faceId, message, ticks, false, true);
            message = null;
        }

        if (getReceiveDict.Count != 0)
        {
            foreach (var item in getReceiveDict)
            {
                SetChatItem(faceId, item.Item1, item.Item2, false, true);
            }
            getReceiveDict.Clear();
        }
    }

    /// <summary>
    /// 聊天界面的返回按钮
    /// </summary>
    private void BackBtnClick()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.MainPanel);
    }

    /// <summary>
    ///  入栈
    /// </summary>
    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        chatReceiveRequest.Init();
        EnterAnimation();

        Invoke("SetItem", 0.5f);
    }

    private void SetItem()
    {
        string chatsStr = PlayerPrefs.GetString(Facade.GetUserData().LoginId.ToString() + "," + friendId.ToString() + "messages");
        if (!string.IsNullOrEmpty(chatsStr))
        {
            messages = GetListByString(chatsStr);
            messages.RemoveAt(messages.Count - 1);
            int i = messages.Count >= 10 ? messages.Count - 10 : 0;
            for (; i < messages.Count; i++)
            {
                string[] msg = messages[i].Split(new string[] { "},{" }, StringSplitOptions.None);
                int faceId = int.Parse(msg[0]) == friendId ? this.faceId : Facade.GetUserData().FaceId;
                long ticks = long.Parse(msg[1]);
                string message = msg[2];
                SetChatItem(faceId, message, ticks, faceId != this.faceId, false);
            }
            getAllReciveMsgs.SendRequest(Facade.GetUserData().LoginId + "," + friendId);
        }
    }

    /// <summary>
    /// Panel退出
    /// </summary>
    public override void OnExit()
    {
        while (chatItems.Count > 0)
        {
            GameObject go = chatItems[0];
            go.SetActive(false);
            chatItems.RemoveAt(0);
            GameObject.DestroyImmediate(go);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, 50);
        chatReceiveRequest.OnDestroy();
        HideAnimation();
    }

    /// <summary>
    /// Panel进入动画
    /// </summary>
    public void EnterAnimation()
    {
        transform.localPosition = new Vector3(300, 0, 0);
        transform.DOLocalMoveX(0, 0.4f);
    }

    /// <summary>
    /// Panel出栈动画
    /// </summary>
    public void HideAnimation()
    {
        Tween tween = transform.DOLocalMoveX(300, 0.4f);
    }

    /// <summary>
    /// 发送按钮点击
    /// </summary>
    private void SendBtnClick()
    {
        try
        {
            int id = Facade.GetUserData().LoginId;
            string message = input.text;
            input.text = "";
            DateTime datetime = DateTime.Now;
            DateTime epoc = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan delta = default(TimeSpan);
            delta = datetime.Subtract(epoc);
            long ticks = (long)delta.TotalMilliseconds;
            string data = id + "," + friendId + "," + message + "," + ticks;
            SetChatItem(Facade.GetUserData().FaceId, message, ticks, true, true);

            Facade.GetUnreadFriendMsg().Add(friendId, (faceId, nickName.text, message, ticks.ToString()));

            chatSendRequest.SendRequest(data);
        }
        catch (Exception ex)
        {
            uiMng.ShowMessage("未知错误" + ex.Message);
        }
    }

    /// <summary>
    /// 设置聊天消息
    /// </summary>
    private void SetChatItem(int faceId, string message, long ticks, bool isSelf, bool isFirst)
    {
        GameObject go;

        if (isSelf)
        {
            go = Instantiate(Resources.Load<GameObject>("Item/MyselfMessageItem"));
        }
        else
        {
            go = Instantiate(Resources.Load<GameObject>("Item/FriendMessageItem"));
        }
        chatItems.Add(go);

        // 设置子物体属性
        DateTime sendTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        sendTime = sendTime.AddMilliseconds(ticks);
        string dateStr = sendTime.ToLongTimeString();
        if (sendTime.Day < DateTime.Now.Day || sendTime.Month < DateTime.Now.Month || sendTime.Year < DateTime.Now.Year)
        {
            dateStr = sendTime.Month + "." + sendTime.Day + sendTime.ToLongTimeString();
            if (sendTime.Year < DateTime.Now.Year)
            {
                dateStr = sendTime.Year + "." + sendTime.Month + "." + sendTime.Day + sendTime.ToLongTimeString();
            }
        }

        go.transform.Find("Message").GetComponent<Text>().text = "<color=\"#556688\">" + dateStr + "</color>" + "\n" + message;

        string facePath = "FaceImage/" + faceId;
        Sprite face = Resources.Load<Sprite>(facePath);
        go.transform.Find("FaceMask/Image").GetComponent<Image>().sprite = face;

        // 设置父物体
        go.transform.SetParent(content, false);

        string dataChat = "";
        int id = isSelf == true ? Facade.GetUserData().LoginId : friendId;
        dataChat = id + "},{" + ticks + "},{" + message;
        if (isFirst)
            messages.Add(dataChat);
        if (messages.Count >= 100)
        {
            messages.RemoveAt(0);
        }
        PlayerPrefs.SetString(Facade.GetUserData().LoginId.ToString() + "," + friendId.ToString() + "messages", GetStringByList(messages));
        
        StartCoroutine("InsSrollBar");
        StartCoroutine("SetDelta", go);
    }

    IEnumerator InsSrollBar()
    {
        yield return new WaitForEndOfFrame();
        scrollbar.value = 0;
    }

    IEnumerator SetDelta(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        Vector2 size = go.GetComponent<RectTransform>().sizeDelta;
        float y = go.transform.Find("Message").GetComponent<RectTransform>().sizeDelta.y;
        y = y > 40 ? y : 40;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, y);

        // 设置layout大小
        size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, size.y + 4 + go.GetComponent<RectTransform>().sizeDelta.y);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    private void GetReceive()
    {
        try
        {
            int id = Facade.GetUserData().LoginId;
            string data = id + "," + friendId;
            chatReceiveRequest.SendRequest(data);
        }
        catch (Exception ex)
        {
            uiMng.ShowMessage("未知错误" + ex.Message);
        }
    }

    /// <summary>
    /// 设置需要聊天的好友的三个信息
    /// </summary>
    public void SetDetail(int friendId, string nickName, int faceId)
    {
        this.friendId = friendId;
        this.nickName.text = nickName;
        this.faceId = faceId;
    }

    /// <summary>
    /// 发送消息响应
    /// </summary>
    public void OnResponseChatSend(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            isSend = false;
            uiMng.ShowMessageSync("发送失败");
        }
        else
        {
            isSend = true;
        }
    }

    /// <summary>
    /// 接收消息响应
    /// </summary>
    public void OnResponseChatReceive(ReturnCode returnCode, string message, long ticks)
    {
        if (returnCode == ReturnCode.Success)
        {
            this.message = message;
            this.ticks = ticks;
        }
    }

    public void OnResponseGetReceiveMsgs(ReturnCode returnCode)
    {

    }

    private string GetStringByList(List<string> list)
    {
        string value = "";
        foreach (var item in list)
        {
            if (!string.IsNullOrEmpty(item))
                value += item + "}.{";
        }
        return value;
    }

    private List<string> GetListByString(string value)
    {
        string[] chats = value.Split(new string[] { "}.{" }, StringSplitOptions.None);
        return new List<string>(chats);
    }
}
