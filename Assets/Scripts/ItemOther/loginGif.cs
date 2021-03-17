using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loginGif : MonoBehaviour
{
    /// <summary>
    /// 精灵名字
    /// </summary>
    public Sprite[] spriteName;
    /// <summary>
    /// 序列的长度
    /// </summary>
    public int count;
    /// <summary>
    /// 当前播放标记
    /// </summary>
    private int index;

    public Image sprite;
    /// <summary>
    /// 每张序列播放的间隔
    /// </summary>
    public float interval = 0.2f;
    /// <summary>
    /// 播放一次完整动画的间隔时间
    /// </summary>
    public float rateTime = 0.0f;
    /// <summary>
    /// 是否只播放一次
    /// </summary>
    public bool playOnlyOnce;

    public void Awake()
    {
        sprite = GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {

    }

    public void OnEnable()
    {
        count = spriteName.Length;
        StartCoroutine(ChangeSprite());
    }
    /// <summary>
    /// 循环播放动画
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeSprite()
    {
        while (index < count)
        {
            yield return new WaitForSeconds(interval);
            if (count != 0)
            {
                index++;
                if (index < count)
                {
                    sprite.sprite = spriteName[index];
                }
                else if (index >= count && !playOnlyOnce)
                {
                    yield return new WaitForSeconds(rateTime);
                    index = 0;
                }
            }
        }
    }

    public void OnDisable()
    {
        sprite.sprite = spriteName[0];
        index = 0;
        StopCoroutine(ChangeSprite());
    }
}
