using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 用于获取star以及bloodType数据
public static class DataListHelper
{
    private static string[] starList = null;
    private static string[] bloodTypeList = null;

    private static string starTextPath = Application.dataPath + @"\Data\star.txt";
    private static string bloodTypeTextPath = Application.dataPath + @"\Data\bloodtype.txt";

    /// <summary>
    /// 获取星座列表
    /// </summary>
    public static string[] StarList
    {
        get
        {
            if (starList == null)
            {
                starList = File.ReadAllLines(starTextPath);
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
                bloodTypeList = File.ReadAllLines(bloodTypeTextPath);
            }

            return bloodTypeList;
        }
    }
}
