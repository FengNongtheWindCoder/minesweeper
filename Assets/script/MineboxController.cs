/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     MineboxController.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-09-27
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

public class MineboxController : MonoBehaviour
{
    enum Boxstate { covered,opened,flaged,};
    private bool hasMine = false;
    public bool HasMine
    {
        set
        {
            hasMine = value;
            if (value)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        get { return hasMine; }
    }
    public int surroundingMine = 0;
    public BoardManager boardManager;
    SpriteRenderer spriteRenderer;
    bool isopened = false;
    GameManager gameManager;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isgamefinish)
        {
            if (hasMine)
            {
                spriteRenderer.color = Color.blue;
            }
            enabled = false;
        }
    }
    /// <summary>
    /// onmousedown只能处理左键单击
    /// gui事件用enable不能控制
    /// </summary>
    public void OnMouseDown()
    {
        Debug.Log("click");
        //在已经打开的位置点击无效
        //游戏结束，不论成功失败，不会再继续操作
        if (isopened || gameManager.isgamefinish)
        {
            return;
        }
        //第一次点击处理地雷布局，自己没有雷，自己周围也不会有雷
        if (!gameManager.ismineSetupFinished)
        {
            gameManager.ismineSetupFinished = true;
            boardManager.SetupMine(this);
        }
        if (HasMine) {
            //点击有雷的部分gameover
           gameManager.gameOver();
           return;
        }
        else
        {
            //点击没有雷的部分，增加打开的box数目，check成功状态
            gameManager.openedboxNum++;
            spriteRenderer.color = Color.yellow;
            //将状态标为已经打开
            isopened = true;
        }
        gameManager.checkGameSuccess();
    }

    /// <summary>
    /// 使用这种变通方式处理右键点击
    /// </summary>
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
        }
    }
}
