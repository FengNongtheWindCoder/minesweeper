/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     ChangeRestartButton.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-10-01
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeRestartButton : MonoBehaviour
{
    public Image restartImg;
    public Sprite successSprite;
    public Sprite failedSprite;
    public Sprite normalSprite;
    // Use this for initialization
    void Start()
    {
        GameManager.instance.GameFinishEvent += onGameFinish;
    }
    /// <summary>
    /// 游戏结束时修改一下按钮的样式
    /// </summary>
    /// <param name="finishtype"></param>
    void onGameFinish(string finishtype)
    {
        if (finishtype.ToLower().Equals("success"))
        {
            restartImg.sprite = successSprite;
        }
        else if (finishtype.ToLower().Equals("failed"))
        {
            restartImg.sprite = failedSprite;
        }
    }
    void OnDestroy()
    {
        GameManager.instance.GameFinishEvent -= onGameFinish;
    }
}
