using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldImg : MonoBehaviour
{
    private Image searchImage;
    private InputField inputField;
    private AddFriendPanel addFriendPanel;

    private void Start()
    {
        // 获取物体组件
        searchImage = transform.Find("Placeholder/Image").GetComponent<Image>();
        inputField = GetComponent<InputField>();
    }

    /// <summary>
    /// 监听搜索点击
    /// </summary>
    public void InputFieldClick()
    {
        searchImage.enabled = true;
        searchImage.sprite = Resources.Load<Sprite>("chatImg\\Bg\\search_img_press") ;
    }

    /// <summary>
    /// 监听搜索非选择,清空
    /// </summary>
    public void InputFieldDeClick()
    {
        searchImage.sprite = Resources.Load<Sprite>("chatImg\\Bg\\search_img");
        inputField.text = "";
    }

    /// <summary>
    /// 搜索栏内容改变，图标消失
    /// </summary>
    public void InputFieldChanged()
    {
        // 更改完不为空且搜索栏没选择就使图片消失
        if (inputField.text != "")
        {
            searchImage.enabled = false;
        }
    }

    /// <summary>
    /// 搜索栏编辑完成
    /// </summary>
    public void InputFieldEdit()
    {
        searchImage.enabled = true;
    }

    /// <summary>
    /// 好友申请搜索按钮编辑完成
    /// </summary>
    public void FriendSearchInputFieldSubmit()
    {
        if (addFriendPanel == null)
        {
            addFriendPanel = GetComponentInParent<AddFriendPanel>();
        }
        searchImage.enabled = true;
        inputField.text = "";
        addFriendPanel.OnSearchFriend();
    }
}
