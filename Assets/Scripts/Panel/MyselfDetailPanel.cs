using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyselfDetailPanel : BasePanel
{
    // 各类显示UI
    private Text nickName;
    private Text idText;
    private Text sex;
    private Text age;
    private Text star;
    private Text bloodType;
    private Text realName;
    private Image faceImage;

    private Button modifyDeatilBtn;

    private ScreenSwipe screenSwipe;  // 控制panel移动的脚本

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        nickName = transform.Find("NickName").GetComponent<Text>();
        idText = transform.Find("IdText").GetComponent<Text>();
        sex = transform.Find("bg-down/Sex").GetComponent<Text>();
        age = transform.Find("bg-down/Age").GetComponent<Text>();
        star = transform.Find("bg-down/Star").GetComponent<Text>();
        bloodType = transform.Find("bg-down/Blood").GetComponent<Text>();
        realName = transform.Find("bg-down/RealName").GetComponent<Text>();
        faceImage = transform.Find("FaceMask/Image").GetComponent<Image>();

        modifyDeatilBtn = transform.Find("Button").GetComponent<Button>();

        screenSwipe = GetComponent<ScreenSwipe>();

        // 添加事件
        modifyDeatilBtn.onClick.AddListener(OnClickModifyBtn);

        // 为组件设置值
        UserData userData = Facade.GetUserData();
        nickName.text = userData.NickName;
        idText.text = "账号 : " + userData.LoginId;
        sex.text = "性别 : " + userData.Sex;
        age.text = "年龄 : " + userData.Age;
        star.text = "星座 : " + DataListHelper.StarList[userData.StarId - 1];
        bloodType.text = "血型 : " + DataListHelper.BloodTypeList[userData.BloodTypeId - 1];
        realName.text = "真实姓名 : " + userData.Name;

        // 给头像赋值
        string facePath = "FaceImage/" + Facade.GetUserData().FaceId;
        Sprite faceImg = Resources.Load<Sprite>(facePath);
        faceImage.sprite = faceImg;

        uiMng.PushPanel(UIPanelType.MainPanel);
    }

    /// <summary>
    /// panel创建时为uiMng赋值
    /// </summary>
    public override void OnEnter()
    {
        uiMng.InjectMyselfDetailPanel(this);
    }

    /// <summary>
    /// 暂停时该script不能使用，以免移动该panel
    /// </summary>
    public void OnPauseMDP()
    {
        base.OnPause();
        screenSwipe.enabled = false;
    }

    /// <summary>
    /// 重启时启动该脚本
    /// </summary>
    public void OnResumeMDP()
    {
        base.OnResume();
        screenSwipe.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 编辑资料点击
    /// </summary>
    private void OnClickModifyBtn()
    {
        uiMng.PushPanel(UIPanelType.ModifyDetailPanel);
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
            this.transform.localPosition = new Vector3(-300 + offset, 0, 0);
        }
        else
        {
            offset = offset < -300 ? -300 : offset;
            // 当panel在中间才能向左滑
            if (this.transform.localPosition.x > -300)
            {
                this.transform.localPosition = new Vector3(offset, 0, 0);
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
            this.transform.DOLocalMoveX(0, 0.2f);
        }
        else
        {
            Tween t = this.transform.DOLocalMoveX(-300, 0.2f);
            // t.OnComplete(() => gameObject.SetActive(false));
        }
    }
}
