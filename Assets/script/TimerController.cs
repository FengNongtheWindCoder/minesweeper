/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     TimerController.cs
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

public class TimerController : MonoBehaviour
{
    GameManager gameManager;
    public Text timertext;
    int timecount = 0;
    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
        gameManager.GameFinishEvent += onGameFinish;
        gameManager.GameStartEvent += onGameStart;
    }
    void OnDestroy()
    {
        gameManager.GameFinishEvent -= onGameFinish;
        gameManager.GameStartEvent -= onGameStart;
    }
    /// <summary>
    /// 监听游戏启动事件并开始计时
    /// </summary>
    void onGameStart()
    {
        timecount = 0;
        StartCoroutine("timer");
    }
    /// <summary>
    /// 游戏结束时停止计时
    /// </summary>
    /// <param name="finishtype"></param>
    void onGameFinish(string finishtype)
    {
        StopCoroutine("timer");
    }
    /// <summary>
    /// 启动后，每隔1s修改一次数字显示
    /// </summary>
    /// <returns></returns>
    IEnumerator timer()
    {
        while (true)
        {
            string minutes = (timecount / 60).ToString().PadLeft(2, '0');
            string sec = (timecount % 60).ToString().PadLeft(2, '0');
            timertext.text = minutes + ":" + sec;
            timecount++;
            //set clock
            yield return new WaitForSeconds(1);
        }

    }

}
