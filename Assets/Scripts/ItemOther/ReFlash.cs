using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReFlash : MonoBehaviour
{
    private Transform content;
    private Vector3 startPos;

    private Text showText;
    private Text timeText;

    // 控制更新字体的位置
    // Start is called before the first frame update
    void Start()
    {
        content = transform.parent.Find("Content");
        startPos = transform.position;

        showText = this.transform.Find("showText").GetComponent<Text>();
        timeText = this.transform.Find("timeText").GetComponent<Text>();

        timeText.text = PlayerPrefs.GetString(Facade.Instance.GetUserData().LoginId + ",ReTime") is null ? "暂无" : PlayerPrefs.GetString(Facade.Instance.GetUserData().LoginId + ",ReTime");
    }

    // Update is called once per frame
    void Update()
    {
        if (content.localPosition.y < -10)
        {
            if (content.localPosition.y < - 100)
            {
                showText.text = "释放立即更新";
            }

            this.transform.position = new Vector3(startPos.x, startPos.y + content.localPosition.y, startPos.z);
        }
        else
        {
            this.transform.position = startPos;
            showText.text = "下拉更新";
            timeText.text = PlayerPrefs.GetString(Facade.Instance.GetUserData().LoginId + ",ReTime") is null ? "暂无" : PlayerPrefs.GetString(Facade.Instance.GetUserData().LoginId + ",ReTime");
        }
    }
}
