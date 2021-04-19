using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    private Button backBtn;
    private Text nickName;

    private int friendId;
    private int faceId;

    // Start is called before the first frame update
    void Awake()
    {
        // 获取组件
        backBtn = this.transform.Find("ChatTopColumn/back").GetComponent<Button>();
        nickName = this.transform.Find("ChatTopColumn/nickName").GetComponent<Text>();

        // 设置事件
        backBtn.onClick.AddListener(BackBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    /// 设置需要聊天的好友的三个信息
    /// </summary>
    public void SetDetail(int friendId, string nickName, int faceId)
    {
        this.friendId = friendId;
        this.nickName.text = nickName;
        this.faceId = faceId;
    }
}
