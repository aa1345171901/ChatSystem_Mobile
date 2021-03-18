using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    private Text text;

    private float showTime = 1;
    private string message = null;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        base.OnEnter();
        text = GetComponent<Text>();
        text.enabled = false;
        uiMng.InjectMsgPanel(this);
    }

    /// <summary>
    /// 每帧调用
    /// </summary>
    private void Update()
    {
        if (message != null)
        {
            ShowMessage(message);
            message = null;
        }
    }

    /// <summary>
    /// 异步显示消息，设置局部变量显示
    /// </summary>
    public void ShowMessageSync(string msg)
    {
        message = msg;
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    public void ShowMessage(string msg)
    {
        gameObject.SetActive(true);

        // 设置messagepanel为最后渲染
        int count = transform.parent.childCount - 1;//Panel移位
        transform.SetSiblingIndex(count);//Panel移位

        text.CrossFadeAlpha(1, 0.5f, false);
        text.text = msg;
        text.enabled = true;
        Invoke("Hide", showTime);
    }

    /// <summary>
    /// 字体渐隐
    /// </summary>
    private void Hide()
    {
        text.CrossFadeAlpha(0, 1, false);
        gameObject.SetActive(false);
    }
}
