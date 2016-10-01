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
    enum BoxState { covered, opened, flaged, questioned };//支持的状态
    enum UserAction { leftclick, rightclick, doubleleftclick };//支持的用户操作
    BoxState currentState;//当前用户状态
    public bool isChangingState = false;//在状态转变一段时间内不允许操作
    public float statechangedelay = .3f;//设置不允许操作的时间长度
    private bool hasMine = false;
    public bool HasMine
    {
        set
        {
            hasMine = value;
        }
        get { return hasMine; }
    }
    public int surroundingMine = 0;
    public BoardManager boardManager;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;
    public Sprite[] numberSprites;
    public Sprite mineSprite;
    public Sprite flagSprite;
    public Sprite questionSprite;
    public SpriteRenderer markerRenderer;
    public bool ismouseover = false;
    public float doubleClickDelay = 0.25f;
    public float lastClicktime = 0;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;
        currentState = BoxState.covered;
    }

    /// <summary>
    /// 处理鼠标事件
    /// </summary>
    public void OnGUI()
    {
        return;
        Event e = Event.current;
        if (!ismouseover || e.type != EventType.MouseDown || gameManager.isgamefinish)
        {
            return;
        }
        if (e.button == 0)
        {
            Debug.Log(e.clickCount);
            if (e.clickCount == 1)
            {
                Debug.Log(" leftclick");
                changeState(UserAction.leftclick);
            }
            else if (e.clickCount == 2)
            {
                Debug.Log(" doubleleftclick");
                changeState(UserAction.doubleleftclick);
            }
        }
        else if (e.button == 1)
        {
            Debug.Log(" rightclick");
            changeState(UserAction.rightclick);
        }
    }
    /// <summary>
    /// 按照给定的时间，将上次点击的记录删除掉，一般是doubleClickDelay
    /// </summary>
    /// <param name="delaytime"></param>
    /// <returns></returns>
    IEnumerator clearLastClicktime(float delaytime)
    {
        yield return new WaitForSecondsRealtime(delaytime);
        lastClicktime = 0;
    }

    /// <summary>
    /// onmouse系列只能处理左键单击
    /// gui事件用enable不能控制
    /// 在这里处理单击还是双击的逻辑
    /// </summary>

    public void OnMouseUpAsButton()
    {
        //游戏结束，不论成功失败，不会再继续操作
        if (gameManager.isgamefinish)
        {
            return;
        }
        //判断单双击事件
        //基于两次点击的事件差
        //第一次点击后触发单击事件并记录时间
        //第二次点击如果小于阈值视为双击事件，否则视为另一次单击事件
        if (lastClicktime == 0)
        {
            changeState(UserAction.leftclick);
            lastClicktime = Time.unscaledTime;
            StartCoroutine(clearLastClicktime(doubleClickDelay));
            Debug.Log("this is a single click ");
        }
        else if (Time.unscaledTime < (lastClicktime + doubleClickDelay))
        {
            lastClicktime = 0;
            changeState(UserAction.doubleleftclick);
            Debug.Log("this is a double click " + (Time.unscaledTime - lastClicktime));
        }
        else
        {
            //因为有clearLastClicktime，这里可能不会进来
            lastClicktime = 0;
            changeState(UserAction.leftclick);
            Debug.Log("this is a single click ");
        }
    }
    /// <summary>
    /// 使用这种变通方式处理右键点击
    /// </summary>
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            changeState(UserAction.rightclick);
        }
    }
    //检测当前鼠标是否在上方
    public void OnMouseEnter()
    {
        ismouseover = true;
    }
    public void OnMouseExit()
    {
        ismouseover = false;
    }

    /// <summary>
    /// minebox状态机
    /// </summary>
    void changeState(UserAction currentAction)
    {
        //防止连续点击时，出现连续状态迁移
        //在状态之间添加短暂的延时
        //如果有动画的话，可以设置为动画播放的时候不允许操作这个box
        if (isChangingState)
        {
            return;
        }
        switch (currentState)
        {
            case BoxState.covered:
                if (currentAction == UserAction.leftclick)
                {
                    //第一次点击处理地雷布局，自己没有雷，自己周围也不会有雷
                    if (!gameManager.ismineSetupFinished)
                    {
                        gameManager.ismineSetupFinished = true;
                        boardManager.SetupMine(this);
                    }
                    if (HasMine)
                    {
                        //点击有雷的部分gameover
                        gameManager.gameOver();
                        //将显示设置为背景红色，marker为地雷图
                        spriteRenderer.color = Color.red;
                        setMarker(true, mineSprite);
                        return;
                    }
                    else
                    {
                        //点击没有雷的部分，增加打开的box数目，check成功状态
                        gameManager.openedboxNum++;
                        spriteRenderer.sprite = numberSprites[surroundingMine];
                        //将状态标为已经打开
                        currentState = BoxState.opened;
                        gameManager.checkGameSuccess();
                    }
                }
                else if (currentAction == UserAction.rightclick)
                {
                    //标记flag，推移状态
                    markerRenderer.sprite = mineSprite;
                    markerRenderer.gameObject.SetActive(true);
                    setMarker(true, flagSprite);
                    currentState = BoxState.flaged;
                }

                break;
            case BoxState.opened:
                //open不处理任何状态推移,只是在双击的时候判断是不是打开周围的邻居
                //只在当前邻居标记的flag数目等于邻居地雷数目时，可以打开周围邻居

                break;
            case BoxState.flaged:
                //flag 状态只处理右键点击
                if (currentAction != UserAction.rightclick)
                {
                    break;
                }
                setMarker(true, questionSprite);
                currentState = BoxState.questioned;
                break;
            case BoxState.questioned:
                //question 状态只处理右键点击
                if (currentAction != UserAction.rightclick)
                {
                    break;
                }
                setMarker(false, null);
                currentState = BoxState.covered;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 修改marker显示的sprite和active状态
    /// </summary>
    /// <param name="markeractive"></param>
    /// <param name="changeto"></param>
    void setMarker(bool markeractive, Sprite changeto)
    {
        markerRenderer.sprite = changeto;
        markerRenderer.gameObject.SetActive(markeractive);
    }
}
