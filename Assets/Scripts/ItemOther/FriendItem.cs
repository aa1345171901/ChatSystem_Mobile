using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    private Button friendBtn;

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        friendBtn = GetComponent<Button>();

        // 设置事件
        friendBtn.onClick.AddListener(BtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    private void BtnClick()
    {
        Facade.Instance.PushPanel(UIPanelType.ChatPanel);
    }
}
