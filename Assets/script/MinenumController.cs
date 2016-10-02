/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     MinenumController.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-10-02
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinenumController : MonoBehaviour {

    GameManager gameManager;
    public Text remainingMinenumtext;
    void Start()
    {
        GameManager.instance.FlagedboxChangeEvent += onRemainingMinenumtextchange;
    }
    void OnDestroy()
    {
        GameManager.instance.FlagedboxChangeEvent -= onRemainingMinenumtextchange;
    }
    /// <summary>
    /// 监听游戏启动事件并开始计时
    /// </summary>
    void onRemainingMinenumtextchange(int newnumber)
    {
        remainingMinenumtext.text = newnumber.ToString();
    }
}
