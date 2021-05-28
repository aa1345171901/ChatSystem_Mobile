using Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FacePanel : BasePanel
{
    private Button back;

    private RectTransform content;

    private SetSystemFaceRequest setSystemFaceRequest;

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        back = transform.Find("TopCloumn/back").GetComponent<Button>();
        content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
        setSystemFaceRequest = GetComponent<SetSystemFaceRequest>();

        // 添加事件
        back.onClick.AddListener(OnClickBack);

        SetFaceIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnter()
    {
        base.OnExit();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnClickBack()
    {
        uiMng.PopPanel();
    }

    /// <summary>
    /// 将可供选择头像显示出来
    /// </summary>
    private void SetFaceIn()
    {
        // 设置layout大小
        Vector2 size = content.sizeDelta;
        content.sizeDelta = new Vector2(size.x, 25 + 160 * 42);

        for (int i = 1; i <= 150; i++)
        {
            GameObject go;
            go = Instantiate(Resources.Load<GameObject>("Item/Face"));

            string facePath = "FaceImage/" + i;
            Sprite face = Resources.Load<Sprite>(facePath);
            go.transform.Find("Image").GetComponent<Image>().sprite = face;
            go.GetComponent<Button>().onClick.AddListener(OnClickFace);

            // 设置父物体
            go.transform.SetParent(content, false);
        }
    }

    /// <summary>
    /// 任意头像点击
    /// </summary>
    public void OnClickFace()
    {
        //通过 UnityEngine.EventSystems的底层来获取到当前点击的对象
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int faceId = int.Parse(button.transform.Find("Image").GetComponent<Image>().sprite.name);
        int dataId = Facade.GetUserData().DataId;
        string data = dataId + "," + faceId;
        setSystemFaceRequest.SendRequest(data);
    }

    public void OnResponseFaceChange(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("头像修改成功。。");
            Thread.Sleep(100);
            uiMng.PopPanelSync();
        }
        else
        {
            uiMng.ShowMessageSync("头像修改失败。。");
        }
    }
}
