using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    // 获取画布的位置
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }

            return canvasTransform;
        }
    }

    private Dictionary<UIPanelType, string> panelPathDict;   //用于存储panel类型与path的对应
    private Dictionary<UIPanelType, BasePanel> panelDict;    //用于存储panel类型与实例的物体的对应
    private Stack<BasePanel> panelStack;                  //用于存储显示的panel

    private MessagePanel msgPanel;     // 用于显示提示信息

    private UIPanelType uIPanelType = UIPanelType.None;   // 用于异步展示panel的标识
    private bool isPop = false;                     // 用于异步出栈的标识

    // 初始化时从UIPanelType的json文件中读取对应的path
    public UIManager(Facade facade):base(facade)
    {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 每帧调用，使用facade的unity生命周期函数
    /// </summary>
    public override void Update()
    {
        if (uIPanelType != UIPanelType.None)
        {
            PushPanel(uIPanelType);
            uIPanelType = UIPanelType.None;
        }

        if (isPop)
        {
            PopPanel();
            isPop = false;
        }
    }

    /// <summary>
    /// 初始化，进栈默认两个panel
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.LoginPanel);
        PushPanel(UIPanelType.MessagePanel);
    }

    /// <summary>
    /// 将实例化的panel存储入栈并显示
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        //当前有显示界面就得进入暂停周期
        if (panelStack.Count > 0)
        {
            //获得栈顶元素而不出栈
            BasePanel panel= panelStack.Peek();
            panel.OnPause();
        }

        BasePanel newPanel = GetPanel(panelType);
        newPanel.OnEnter();
        panelStack.Push(newPanel);
    }

    /// <summary>
    /// 异步展示Panel
    /// </summary>
    public void PushPanelSync(UIPanelType uIPanelType)
    {
        this.uIPanelType = uIPanelType;
    }

    /// <summary>
    /// panel出栈，栈顶的进入OnExit周期，栈第二个的进入OnResume周期，
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if (panelStack.Count > 0)
        {
            BasePanel panel1 = panelStack.Pop();
            panel1.OnExit();
            if (panelStack.Count > 0)
            {
                BasePanel panel2 = panelStack.Peek();
                panel2.OnResume();
            }
        }
    }

    /// <summary>
    /// 异步出栈
    /// </summary>
    public void PopPanelSync()
    {
        isPop = true;
    }


    [Serializable]//序列化对象用于接收json的数据
    class PanelTypePathJson
    {
        public List<UIPanelInfo> infoList;
    }

    /// <summary>
    /// 将json文件存入
    /// </summary>
    private void ParseUIPanelTypeJson () {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        // 只能将数据转换为对象
        PanelTypePathJson jsonObject = JsonUtility.FromJson <PanelTypePathJson>(ta.text);
        foreach (UIPanelInfo info  in jsonObject.infoList)
        {
            panelPathDict.Add(info.panelType, info.path);
        }
	}

    /// <summary>
    /// 实例化请求显示的panel并存入字典
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }
        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);
        if (panel != null)
        {
            return panel;
        }

        string panelPath;
        panelPathDict.TryGetValue(panelType, out panelPath);
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(panelPath));
        go.transform.SetParent(CanvasTransform,false);
        go.GetComponent<BasePanel>().UIMng = this;
        go.GetComponent<BasePanel>().Facade = this.facade;
        panelDict.Add(panelType,go.GetComponent<BasePanel>());
        return go.GetComponent<BasePanel>();
    }

    /// <summary>
    /// messagePanel获取
    /// </summary>
    /// <param name="msgPanel"></param>
    public void InjectMsgPanel(MessagePanel msgPanel)
    {
        this.msgPanel = msgPanel;
    }

    /// <summary>
    /// 同步消息显示
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMessage(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空"); return;
        }
        msgPanel.ShowMessage(msg);
    }

    /// <summary>
    /// 异步
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMessageSync(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空"); return;
        }
        msgPanel.ShowMessageSync(msg);
    }
}
