using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendItem : MonoBehaviour
{
    private Text detailData;     // 好友的昵称(账号)
    private Button addFriendBtn;   // 添加好友item后面的添加按钮

    private AddFriendPanel addFriendPanel;

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        addFriendBtn = transform.Find("AddBtn").GetComponent<Button>();
        detailData = transform.Find("NickName").GetComponent<Text>();
        addFriendPanel = transform.GetComponentInParent<AddFriendPanel>();

        // 添加事件
        addFriendBtn.onClick.AddListener(OnAddFriendBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 添加好友按钮点击
    /// </summary>
    private void OnAddFriendBtnClick()
    {
        addFriendPanel.OnClickAddFriendItem(detailData.text);
    }
}
