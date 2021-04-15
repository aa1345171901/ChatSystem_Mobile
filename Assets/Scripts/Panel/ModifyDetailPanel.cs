using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyDetailPanel : BasePanel
{
    private Dropdown sexDropDown;
    private Dropdown starDropDown;
    private Dropdown bloodTypeDropDown;

    private InputField nickNameIF;
    private InputField ageIF;

    private Button back;

    private Button saveBtn;

    private string nickName = "";
    private int age;

    // Start is called before the first frame update
    void Awake()
    {
        // 获取组件
        sexDropDown = transform.Find("Sex/Dropdown").GetComponent<Dropdown>();
        starDropDown = transform.Find("Star/Dropdown").GetComponent<Dropdown>();
        bloodTypeDropDown = transform.Find("Blood/Dropdown").GetComponent<Dropdown>();

        nickNameIF = transform.Find("NickName/InputField").GetComponent<InputField>();
        ageIF = transform.Find("Age/InputField").GetComponent<InputField>();

        back = transform.Find("TopCloumn/back").GetComponent<Button>();
        saveBtn = transform.Find("Button").GetComponent<Button>();

        // 添加事件
        back.onClick.AddListener(OnClickBack);
        saveBtn.onClick.AddListener(OnClickSave);
    }

    private void Start()
    {
        // 为Dropdown设置值
        UpdateDropDownItem(new List<string>() { "男", "女" }, sexDropDown, Facade.GetUserData().Sex == "男" ? 0 : 1);
        UpdateDropDownItem(new List<string>(DataListHelper.StarList), starDropDown, Facade.GetUserData().StarId - 1);
        UpdateDropDownItem(new List<string>(DataListHelper.BloodTypeList), bloodTypeDropDown, Facade.GetUserData().BloodTypeId - 1);
    }

    /// <summary>
    /// panel退出时的事件
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// panel进入时的事件
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        // 为Dropdown设置值
        UpdateDropDownItem(new List<string>() { "男", "女" }, sexDropDown, Facade.GetUserData().Sex == "男" ? 0 : 1);
        UpdateDropDownItem(new List<string>(DataListHelper.StarList), starDropDown, Facade.GetUserData().StarId - 1);
        UpdateDropDownItem(new List<string>(DataListHelper.BloodTypeList), bloodTypeDropDown, Facade.GetUserData().BloodTypeId - 1);
        gameObject.SetActive(true);
    }

    void Update()
    {

    }

    /// <summary>
    /// dropdown更新
    /// </summary>
    /// <param name="showNames"></param>
    private void UpdateDropDownItem(List<string> showNames, Dropdown dropdown, int showIndex)
    {
        dropdown.options.Clear();
        Dropdown.OptionData temoData;
        for (int i = 0; i < showNames.Count; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = showNames[i];
            dropdown.options.Add(temoData);
        }
        //初始选项的显示
        dropdown.captionText.text = showNames[showIndex];
        dropdown.value = showIndex;
    }

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnClickBack()
    {
        uiMng.PopPanel();
    }

    /// <summary>
    /// 保存按钮点击
    /// </summary>
    private void OnClickSave()
    {
        if (nickNameIF.text != "")
        {
            Facade.GetUserData().NickName = nickNameIF.text;
        }
        Facade.GetUserData().Sex = sexDropDown.captionText.text;

        if (ageIF.text != "")
        {
            Facade.GetUserData().Age = int.Parse(ageIF.text);
        }
        Facade.GetUserData().StarId = starDropDown.value + 1;
        Facade.GetUserData().BloodTypeId = bloodTypeDropDown.value + 1;
    }
}
