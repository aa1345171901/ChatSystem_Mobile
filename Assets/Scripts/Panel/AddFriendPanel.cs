using Common;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendPanel : BasePanel
{
    private Button backBtn;

    private SearchFriendRequest searchFriendRequest;  // 查找好友请求
    private AddFriendRequest addFriendRequest;       // 添加好友请求

    private InputField inputField;  // 搜索组件

    private List<List<string>> searchFriends = null;  // 用于保存搜索的好友
    private List<GameObject> friendGOs = new List<GameObject>();   // 列表Item列表，刷新时先隐藏

    private RectTransform content;  // layout的物体大小，用于设置

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        backBtn = this.transform.Find("AddTopCloumn/back").GetComponent<Button>();

        searchFriendRequest = GetComponent<SearchFriendRequest>();
        addFriendRequest = GetComponent<AddFriendRequest>();

        inputField = transform.Find("AddTopCloumn/InputField").GetComponent<InputField>();

        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        // 设置事件
        backBtn.onClick.AddListener(BackBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        // 异步展示搜索到的好友
        if (searchFriends != null)
        {
            SetAddFriendItem();
            // SendMessageUpwards("SetAddFriendPanel", this);
            searchFriends = null;
        }
    }

    /// <summary>
    /// 聊天界面的返回按钮
    /// </summary>
    private void BackBtnClick()
    {
        Facade.Instance.PopPanel();
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

    /// <summary>
    /// 搜索好友事件
    /// </summary>
    public void OnSearchFriend()
    {
        // 搜索栏不为空就按昵称或id查找
        if (!string.IsNullOrEmpty(inputField.text))
        {
            BasicallySearch(inputField.text);
        }
        else
        {
            // 否则随机查找
            RandomSearch();
        }
    }

    /// <summary>
    /// 基本查找
    /// </summary>
    private void BasicallySearch(string searchText)
    {
        if (searchText != "")
        {
            try
            {
                string nickName = searchText;
                int friendId = 0;
                int.TryParse(searchText, out friendId);
                string data = friendId + "," + nickName + ",,";
                searchFriendRequest.SendRequest(data);
            }
            catch (Exception ex)
            {
                uiMng.ShowMessageSync("搜索失败，请检查您的网络");
                Debug.Log("基本搜索：" + ex.Message);
            }
        }
    }

    /// <summary>
    /// 随机查找
    /// </summary>
    private void RandomSearch()
    {
        try
        {
            string data = ",,,";
            searchFriendRequest.SendRequest(data);
        }
        catch (Exception ex)
        {
            uiMng.ShowMessageSync("搜索失败，请检查您的网络");
            Debug.Log("随机搜索：" + ex.Message);
        }
    }

    /// <summary>
    /// 搜索好友的响应
    /// </summary>
    public void OnResponseSearch(ReturnCode returnCode, List<List<string>> lists)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("搜索失败┭┮﹏┭┮");
        }
        else
        {
            searchFriends = lists;
        }
    }

    /// <summary>
    /// 用于设置添加的好友子物体
    /// </summary>
    private void SetAddFriendItem()
    {
        // 设置layout大小
        Vector2 size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, 25 + 60 * (searchFriends.Count + 1));

        int index = 0;
        foreach (var item in searchFriends)
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
                go = Instantiate(Resources.Load<GameObject>("Item/AddFriendItem"));
                friendGOs.Add(go);

                // 不自增只能生成一个物体
                index++;
            }
            string nickName = item[1] + "   [ " + item[0] + " ]";
            int faceId = 0;
            int.TryParse(item[4], out faceId);

            // 设置子物体属性
            go.transform.Find("NickName").GetComponent<Text>().text = nickName;

            string facePath = "FaceImage/" + faceId;
            Sprite face = Resources.Load<Sprite>(facePath);
            go.transform.Find("FaceMask/Image").GetComponent<Image>().sprite = face;

            // 设置父物体
            go.transform.SetParent(content, false);
        }
        // 更新后好友数量少了，将后面的设为不可见
        if (index < friendGOs.Count)
        {
            for (int i = index; i < friendGOs.Count; i++)
            {
                friendGOs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 子物体点击
    /// </summary>
    public void OnFriendItemClick()
    {
        FriendDetailPanel friendDetailPanel = uiMng.PushPanel(UIPanelType.FriendDetailPanel) as FriendDetailPanel;
        friendDetailPanel.btnText.text = "加好友";
    }

    /// <summary>
    /// 点击子物体的添加按钮
    /// </summary>
    public void OnClickAddFriendItem(string input)
    {
        try
        {
            Match m = Regex.Match(input, "\\[( .*? )\\]");
            int friendId = int.Parse(m.Groups[1].Value);
            int id = _facade.GetUserData().LoginId;
            string data = id + "," + friendId;

            addFriendRequest.SendRequest(data);
        }
        catch (System.Exception e)
        {
            Debug.Log("添加失败:" + e.Message);
        }
    }

    /// <summary>
    /// 添加好友请求回应
    /// </summary>
    public void OnResponseAddFriend(ReturnCode returnCode, string result)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("添加成功：请刷新查看");
        }
        else
        {
            uiMng.ShowMessageSync("添加失败：" + result);
        }
    }
}
