using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FriendDetailPanel : BasePanel
{
    private Button back;
    private Button addAndSendBtn;
    private Button deleteBtn;
    private GameObject deletePanel;
    private Button sure;
    private Button cancel;

    public Text btnText;  // 设置底部按钮是加好友或者发消息
    public Text idText;

    // 各类显示UI
    private Text nickName;
    private Text sex;
    private Text age;
    private Text star;
    private Text bloodType;
    private Text realName;
    private Image faceImage;

    public bool isGet = false;   // 判断好友资料是否获得

    private DeleteFriendRequest deleteFriendRequest;  // 删除好友请求

    // Start is called before the first frame update
    void Awake()
    {
        // 获取组件
        back = transform.Find("TopImage/back").GetComponent<Button>();
        deleteBtn = transform.Find("Delete").GetComponent<Button>();
        addAndSendBtn = transform.Find("Button").GetComponent<Button>();
        btnText = transform.Find("Button/Text").GetComponent<Text>();
        nickName = transform.Find("NickName").GetComponent<Text>();
        idText = transform.Find("IdText").GetComponent<Text>();
        sex = transform.Find("bg-down/Sex").GetComponent<Text>();
        age = transform.Find("bg-down/Age").GetComponent<Text>();
        star = transform.Find("bg-down/Star").GetComponent<Text>();
        bloodType = transform.Find("bg-down/Blood").GetComponent<Text>();
        realName = transform.Find("bg-down/RealName").GetComponent<Text>();
        faceImage = transform.Find("FaceMask/Image").GetComponent<Image>();

        deletePanel = transform.Find("DeletePanel").gameObject;
        sure = deletePanel.transform.Find("Sure").GetComponent<Button>();
        cancel = deletePanel.transform.Find("Cancel").GetComponent<Button>();

        deleteFriendRequest = GetComponent<DeleteFriendRequest>();

        // 添加事件
        back.onClick.AddListener(OnClickBack);
        addAndSendBtn.onClick.AddListener(OnClickSendOrAdd);
        deleteBtn.onClick.AddListener(OnClickDelete);
        sure.onClick.AddListener(OnClickSure);
        cancel.onClick.AddListener(OnClickCancel);

        deletePanel.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    public void SetDetail()
    {
        // 为组件设置值
        UserData userData = Facade.GetFriendUserData();
        nickName.text = userData.NickName;
        sex.text = "性别 : " + userData.Sex;
        age.text = "年龄 : " + userData.Age;
        string starStr = userData.StarId - 1 >= 0 ? DataListHelper.StarList[userData.StarId - 1] : "";
        star.text = "星座 : " + starStr;
        string bloodStr = userData.BloodTypeId - 1 >= 0 ? DataListHelper.BloodTypeList[userData.BloodTypeId - 1] : "";
        bloodType.text = "血型 : " + bloodStr;
        realName.text = "真实姓名 : " + userData.Name;

        // 给头像赋值
        string facePath = "FaceImage/" + userData.FaceId;
        Sprite faceImg = Resources.Load<Sprite>(facePath);
        faceImage.sprite = faceImg;
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public override void OnPause()
    {
        base.OnPause();
        gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGet)
        {
            SetDetail();
            isGet = false;
        }
    }

    private void OnClickBack()
    {
        uiMng.PopPanel();
    }

    /// <summary>
    /// 获取好友信息
    /// </summary>
    public void OnGetDetailResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("加载好友消息失败");
        }
        else
        {
            
        }
    }

    /// <summary>
    /// 不同的点击地方进入该panel，该按钮的事件不同
    /// </summary>
    private void OnClickSendOrAdd()
    {
        if (btnText.text == "加好友")
        {
            try
            {
                AddFriendRequest addFriendRequest = transform.parent.GetComponentInChildren<AddFriendRequest>();
                int friendId = int.Parse(idText.text.Split(':')[1]);
                int id = _facade.GetUserData().LoginId;
                string data = id + "," + friendId;

                addFriendRequest.SendRequest(data);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        if (btnText.text == "同意添加")
        {
            try
            {
                AgreeAddRequest agreeAddRequest = transform.parent.GetComponentInChildren<AgreeAddRequest>();
                int friendId = int.Parse(idText.text.Split(':')[1]);
                int id = _facade.GetUserData().LoginId;
                string data = friendId + "," + id;

                agreeAddRequest.SendRequest(data);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            //uiMng.PopPanel();
            ChatPanel chatPanel = uiMng.PushPanel(UIPanelType.ChatPanel) as ChatPanel;
            chatPanel.SetDetail(int.Parse(idText.text.Split(':')[1]), nickName.text, int.Parse(faceImage.sprite.name));
        }
    }

    /// <summary>
    /// 删除按钮点击
    /// </summary>
    private void OnClickDelete()
    {
        deletePanel.SetActive(true);
    }

    /// <summary>
    /// 隐藏delete按钮
    /// </summary>
    public void OnHideDelete()
    {
        deleteBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示delete按钮
    /// </summary>
    public void OnSetDelete()
    {
        deleteBtn.gameObject.SetActive(true);
    }

    private void OnClickSure()
    {
        try
        {
            deletePanel.SetActive(false);
            int hostFriendId = Facade.GetUserData().LoginId;
            int accetFriendId = int.Parse(idText.text.Split(':')[1]);
            string data = hostFriendId.ToString() + "," + accetFriendId.ToString();
            deleteFriendRequest.SendRequest(data);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void OnClickCancel()
    {
        deletePanel.SetActive(false);
    }

    public void OnResponseDelete(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("删除失败，请稍后重试");
        }
        else
        {
            uiMng.ShowMessageSync("删除成功，正在刷新好友列表");
            uiMng.PopPanelSync();
            //GetFriendListRequest getFriendListRequest = transform.parent.GetComponentInChildren<GetFriendListRequest>();
            //getFriendListRequest.SendRequest(Facade.GetUserData().GetId.ToString());
        }
    }
}
