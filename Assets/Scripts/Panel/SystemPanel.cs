using Common;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SystemPanel : BasePanel
{
    private Button back;

    private RectTransform content;  // layout的物体大小，用于设置

    private AgreeAddRequest agreeAddRequest;
    private AddFriendMessageRequest addFriendMsgRequest;

    private List<GameObject> friendAddRequest = new List<GameObject>();
    private string friendData;  // 保存本地好友请求数据。
    public Dictionary<int, (string, int)> friendDict = new Dictionary<int, (string, int)>();
    public bool isHaveUnread = false;

    private Dictionary<int, (string, int)> FriendDic = new Dictionary<int, (string, int)>();  // 增加到好友列表，是添加变已添加
    public bool isAdd = false;

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        back = transform.Find("TopCloumn/back").GetComponent<Button>();
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        agreeAddRequest = GetComponent<AgreeAddRequest>();
        addFriendMsgRequest = GetComponent<AddFriendMessageRequest>();

        string friendsStr = PlayerPrefs.GetString(Facade.GetUserData().LoginId + ",friends");
        if (!string.IsNullOrEmpty(friendsStr))
        {
            FriendDic = DataHelper.StringToDic(friendsStr);
        }

        // 获取本地记录，记录10条
        friendData = PlayerPrefs.GetString(Facade.GetUserData().LoginId + "AgreeSystem");
        if (!string.IsNullOrEmpty(friendData))
        {
            string[] strs = friendData.Split('-');
            for(int i = 0; i < strs.Length - 1; i++)
            {
                int id = int.Parse(strs[i].Split(',')[0]);
                string nickName = strs[i].Split(',')[1];
                int faceid = int.Parse(strs[i].Split(',')[2]);
                friendDict.Add(id, (nickName, faceid));
            }
            SetFriendRequestItem();
        }

        // 添加事件
        back.onClick.AddListener(BackClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Facade.GetUnreadSystemMsg().Count > 0)
        {
            Dictionary<int, int> systemMsg = Facade.GetUnreadSystemMsg();
            foreach (var item in systemMsg)
            {
                if (!friendDict.ContainsKey(item.Key))
                {
                    friendDict.Add(item.Key, ("", item.Value));
                }
            }
            GetSystemMsg();
            Facade.ClearSystemMsg();
        }
        if (isHaveUnread)
        {
            SetFriendRequestItem();
            PlayerPrefs.SetString(Facade.GetUserData().LoginId + "AgreeSystem", friendData);
            isHaveUnread = false;
        }
        if (isAdd)
        { 
             PlayerPrefs.SetString(Facade.GetUserData().LoginId + ",friends", DataHelper.DicToString(FriendDic));
        }
        else
        {
            FriendDic.Remove(FriendDic.Count - 1);
        }
    }

    /// <summary>
    ///  入栈
    /// </summary>
    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        EnterAnimation();
    }

    /// <summary>
    /// Panel退出
    /// </summary>
    public override void OnExit()
    {
        HideAnimation();
    }

    public override void OnPause()
    {
        base.OnPause();
        transform.localPosition = new Vector3(300, 0, 0);
    }

    public override void OnResume()
    {
        base.OnResume();
        transform.localPosition = new Vector3(0, 0, 0);
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
        tween.OnComplete(() => gameObject.SetActive(false));
    }

    private  void BackClick()
    {
        uiMng.PopPanel();
    }

    private void SetFriendRequestItem()
    {
        // 设置layout大小
        Vector2 size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, 25 + 40 * (friendDict.Count + 1));

        friendData = "";

        int index = friendAddRequest.Count - 1;

        int[] keys = new int[friendDict.Count];
        int j = 0;
        foreach (int key in friendDict.Keys) keys[j++] = key;

        int i = friendDict.Count - 10 > 0 ? friendDict.Count - 10 : 0;
        for(; i < friendDict.Count; i++)
        {
            friendDict.TryGetValue(keys[i], out (string, int) item);
            friendData += keys[i] + "," + item.Item1 + "," + item.Item2 + "-";
            // friendGOs里有就不用生成
            GameObject go;
            if (index >= 0)
            {
                go = friendAddRequest[index--];
            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>("Item/AddRequestItem"));
                friendAddRequest.Add(go);
            }
            string nickName = item.Item1 + "   [ " + keys[i] + " ]";
            int faceId = item.Item2;

            // 设置子物体属性
            go.transform.Find("NickName").GetComponent<Text>().text = nickName;
            go.name = keys[i].ToString();

            string facePath = "FaceImage/" + faceId;
            Sprite face = Resources.Load<Sprite>(facePath);
            go.transform.Find("FaceMask/Image").GetComponent<Image>().sprite = face;

            if (FriendDic.ContainsKey(keys[i]))
            {
                go.transform.Find("AddBtn").GetComponent<Text>().text = "已同意";
            }
            else
            {
                go.transform.Find("AddBtn").GetComponent<Button>().onClick.RemoveAllListeners();
                go.transform.Find("AddBtn").GetComponent<Button>().onClick.AddListener(OnItemClick);
            }

            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(OnFriendItemClick);
            // 设置父物体
            go.transform.SetParent(content, false);
        }
    }

    /// <summary>
    /// 子物体点击
    /// </summary>
    public void OnFriendItemClick()
    {
        FriendDetailPanel friendDetailPanel = uiMng.PushPanel(UIPanelType.FriendDetailPanel) as FriendDetailPanel;
        GetFriendDetailRequest getDetail = friendDetailPanel.GetComponent<GetFriendDetailRequest>();

        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string str = button.transform.Find("NickName").GetComponent<Text>().text;
        Match m = Regex.Match(str, "\\[( .*? )\\]");
        int friendId = int.Parse(m.Groups[1].Value);

        friendDetailPanel.idText.text = "账号 :" + friendId.ToString();
        if (FriendDic.ContainsKey(friendId))
        {
            friendDetailPanel.btnText.text = "发消息";
        }
        else
        {
            friendDetailPanel.btnText.text = "同意添加";
            friendDetailPanel.OnSetDelete();
            friendDetailPanel.OnHideDelete();
        }

        // 获取详细信息请求
        getDetail.SendRequest(friendId.ToString());
    }

    /// <summary>
    /// 同意添加按钮点击
    /// </summary>
    private void OnItemClick()
    {
        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string str = button.transform.parent.Find("NickName").GetComponent<Text>().text;
        Match m = Regex.Match(str, "\\[( .*? )\\]");
        int friendId = int.Parse(m.Groups[1].Value);

        int accetFriendId = Facade.GetUserData().LoginId;
        string data = friendId + "," + accetFriendId;      

        string nickName = button.transform.parent.Find("NickName").GetComponent<Text>().text;
        int faceId = int.Parse(button.transform.parent.Find("FaceMask/Image").GetComponent<Image>().sprite.name);
        if (!FriendDic.ContainsKey(friendId))
        {
            FriendDic.Add(friendId, (nickName, faceId));
        }    

        agreeAddRequest.SendRequest(data);

        button.GetComponent<Text>().text = "已同意";
    }

    /// <summary>
    /// 同意添加好友响应
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="result"></param>
    public void OnResponseAgree(ReturnCode returnCode, string result)
    {
        uiMng.ShowMessageSync(result);
    }

    /// <summary>
    /// 获取好友信息
    /// </summary>
    private void GetSystemMsg()
    {
        try
        {
            lock (friendDict)
            {
                foreach (var item in friendDict)
                {
                    if (item.Value.Item1 == "")
                    {
                        int id = Facade.GetUserData().LoginId;
                        int fromUserId = item.Key;
                        string data = id + "," + fromUserId;
                        addFriendMsgRequest.SendRequest(data);
                        Thread.Sleep(100);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// 获取好友请求消息响应
    /// </summary>
    public void OnResponseGetRequest(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("获取信息失败。。");
        }
    }
}
