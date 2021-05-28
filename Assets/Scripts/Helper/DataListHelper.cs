using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 用于获取star以及bloodType数据
public static class DataListHelper
{
    private static string[] starList = null;
    private static string[] bloodTypeList = null;

    private static string starTextPath = "Data/star";
    private static string bloodTypeTextPath = "Data/bloodtype";

    /// <summary>
    /// 获取星座列表
    /// </summary>
    public static string[] StarList
    {
        get
        {
            if (starList == null)
            {
                TextAsset str = Resources.Load<TextAsset>(starTextPath);
                starList = str.text.Split('\n');
            }
            return starList;
        }
    }

    /// <summary>
    /// 获取血型列表
    /// </summary>
    public static string[] BloodTypeList
    {
        get
        {
            if (bloodTypeList == null)
            {
                string str = Resources.Load<TextAsset>(bloodTypeTextPath).ToString();
                bloodTypeList = str.Split('\n');
            }

            return bloodTypeList;
        }
    }
}
