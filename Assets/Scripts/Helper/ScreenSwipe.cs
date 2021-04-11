using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwipe : MonoBehaviour
{
    private float fingerActionSensitivity = Screen.width * 0.4f; //手指划过40%屏幕宽度触发事件.

    private float fingerBeginX;    // 屏幕点击时x坐标
    private float fingerBeginY;    // 屏幕点击时y坐标
    private float fingerCurrentX;  // 屏幕拖动时x当前坐标
    private float fingerCurrentY;  // 屏幕拖动时y当前坐标
    private float fingerSegmentX;  // 屏幕松点击时 坐标
    private float fingerSegmentY;

    // 记录手指状态
    private int fingerTouchState;
    // 手指状态枚举
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;

    void Start()
    {

        fingerActionSensitivity = Screen.width * 0.4f;

        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;

        fingerTouchState = FINGER_STATE_NULL;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (fingerTouchState == FINGER_STATE_NULL)
            {
                fingerBeginY = Input.mousePosition.y;
                fingerBeginX = Input.mousePosition.x;
                if (fingerBeginY < 80)
                {
                    return;
                }
                fingerTouchState = FINGER_STATE_TOUCH;
            }
        }

        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            fingerCurrentX = Input.mousePosition.x;
            fingerCurrentY = Input.mousePosition.y;
            fingerSegmentX = fingerCurrentX - fingerBeginX;
            fingerSegmentY = fingerCurrentY - fingerBeginY;

            // 右滑过程中，发送消息给其他panel进行滑动
            SendMessage("OnFingerMove", fingerSegmentX);
        }

        // 松开手指或鼠标
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // 触发事件
            toAddFingerAction();

            // 触发事件后，重新设置为没有点击
            fingerTouchState = FINGER_STATE_NULL;
        }
    }

    private void toAddFingerAction()
    {

        fingerTouchState = FINGER_STATE_ADD;

        // 屏幕滑动超过设定目标时
        if (fingerSegmentX > fingerActionSensitivity)
        {
            SendMessage("OnFingerAction", true);
        }
        if (fingerSegmentX < -fingerActionSensitivity)
        {
            SendMessage("OnFingerAction", false);
        }
        if (fingerSegmentX < fingerActionSensitivity && fingerSegmentX > 0)
        {
            SendMessage("OnFingerAction", false);
        }
        if (fingerSegmentX > -fingerActionSensitivity && fingerSegmentX < 0)
        {
            SendMessage("OnFingerAction", true);
        }

    }
}
