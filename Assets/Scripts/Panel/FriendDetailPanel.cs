using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendDetailPanel : BasePanel
{
    private Button back;
    private Button addAndSendBtn;

    public Text btnText;  // 设置底部按钮是加好友或者发消息
    private Text idText;

    // Start is called before the first frame update
    void Awake()
    {
        // 获取组件
        back = transform.Find("TopImage/back").GetComponent<Button>();
        addAndSendBtn = transform.Find("Button").GetComponent<Button>();
        btnText = transform.Find("Button/Text").GetComponent<Text>();
        idText = transform.Find("IdText").GetComponent<Text>();

        // 添加事件
        back.onClick.AddListener(OnClickBack);
        addAndSendBtn.onClick.AddListener(OnClickSendOrAdd);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickBack()
    {
        uiMng.PopPanel();
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
                int friendId = int.Parse(idText.text.Split('：')[1]);
                int id = _facade.GetUserData().LoginId;
                string data = id + "," + friendId;

                addFriendRequest.SendRequest(data);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            uiMng.PopPanel();
            uiMng.PushPanel(UIPanelType.ChatPanel);
        }
    }
}
