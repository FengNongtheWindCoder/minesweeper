/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     GameManager.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-09-27
 *Description:   负责处理游戏开始结束的逻辑
 *History:  
**********************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //保持此脚本场景中唯一，也是保持脚本所在的GameManager对象场景唯一的手段
    public static GameManager instance;
    //获取游戏具体逻辑控制
    public BoardManager boardManager;
    //地图需要的几个参数
    public int size_x = 9;
    public int size_y = 9;
    public int mine_num = 10;
    public int safeboxNum = 0;
    public int openedboxNum = 0;
    public bool ismineSetupFinished = false;
    public bool isgamefinish = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //保持场景切换不删除
        DontDestroyOnLoad(gameObject);
        boardManager = gameObject.GetComponent<BoardManager>();
        InitGame();
    }
    /// <summary>
    /// reset data
    /// </summary>
    void OnLevelWasLoaded()
    {
        openedboxNum = 0;
        ismineSetupFinished = false;
        isgamefinish = false;
        InitGame();
    }

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// Initializes the game.
    /// </summary>
    void InitGame()
    {
        safeboxNum = size_x * size_y - mine_num;
        boardManager.SetupEmpty(size_x, size_y, mine_num);
    }


    public bool checkGameSuccess()
    {
        if (openedboxNum == safeboxNum)
        {
            Debug.Log("game success");
            isgamefinish = true;
            return true;
        }
        return false;
    }
    public void gameOver()
    {
        isgamefinish = true;
        Debug.Log("game over");
    }
}
