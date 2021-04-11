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

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        sexDropDown = transform.Find("Sex/Dropdown").GetComponent<Dropdown>();
        starDropDown = transform.Find("Star/Dropdown").GetComponent<Dropdown>();
        bloodTypeDropDown = transform.Find("Blood/Dropdown").GetComponent<Dropdown>();

        UpdateDropDownItem(new List<string>() { "男","女"}, sexDropDown);
        UpdateDropDownItem(new List<string>(DataListHelper.StarList), starDropDown);
        UpdateDropDownItem(new List<string>(DataListHelper.BloodTypeList), bloodTypeDropDown);
    }

    void Update()
    {

    }

    /// <summary>
    /// dropdown更新
    /// </summary>
    /// <param name="showNames"></param>
    private void UpdateDropDownItem(List<string> showNames, Dropdown dropdown)
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
        dropdown.captionText.text = showNames[0];
    }
}
