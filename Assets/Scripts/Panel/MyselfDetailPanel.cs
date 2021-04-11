using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyselfDetailPanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 跟随滑动
    /// </summary>
    /// <param name="offset"></param>
    public void OnFingerMove(int offset)
    {
        // 向右滑
        if (offset > 0)
        {
            offset = offset > 300 ? 300 : offset;
            this.transform.localPosition = new Vector3(-300 + offset, 0, 0);
        }
        else
        {
            offset = offset < -300 ? -300 : offset;
            // 当panel在中间才能向左滑
            if (this.transform.localPosition.x > -300)
            {
                this.transform.localPosition = new Vector3(offset, 0, 0);
            }
        }
    }

    /// <summary>
    /// 手指右滑事件,判断是否满足右滑距离
    /// </summary>
    public void OnFingerAction(bool isMove)
    {
        if (isMove)
        {
            this.transform.DOLocalMoveX(0, 0.2f);
        }
        else
        {
            Tween t = this.transform.DOLocalMoveX(-300, 0.2f);
            // t.OnComplete(() => gameObject.SetActive(false));
        }
    }
}
