using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SystemPanel : BasePanel
{
    private Button back;

    private RectTransform content;  // layout的物体大小，用于设置

    private AgreeAddRequest agreeAddRequest;

    private float timer = 0;   // 每一秒查看是否有未读数据
    private string friendData;  // 保存本地好友请求数据。

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        back = transform.Find("TopCloumn/back").GetComponent<Button>();
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

        agreeAddRequest = GetComponent<AgreeAddRequest>();

        // 添加事件
        back.onClick.AddListener(BackClick);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            if (Facade.GetUnreadMsg() != null)
            {
                SetFriendRequestItem();
                PlayerPrefs.SetString(Facade.GetUserData().LoginId + "AgreeSystem",friendData);
            }
            timer = 0;
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

    private void  SetFriendRequestItem()
    {
    
    }


    /// <summary>
    /// 同意添加按钮点击
    /// </summary>
    private void OnItemClick()
    {
        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string str = button.transform.Find("NickName").GetComponent<Text>().text;
        Match m = Regex.Match(str, "\\[( .*? )\\]");
        int friendId = int.Parse(m.Groups[1].Value);

        int accetFriendId = Facade.GetUserData().LoginId;
        string data = friendId + "," + accetFriendId;

        agreeAddRequest.SendRequest(data);
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
}
