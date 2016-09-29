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
    enum BoxState { covered, opened, flaged, questioned };
    enum UserAction { nothing, leftclick, rightclick };
    BoxState currentState;
    UserAction currentAction;
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
    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;
        currentState = BoxState.covered;
        currentAction = UserAction.nothing;
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
        if (gameManager.isgamefinish)
        {
            return;
        }
        //第一次点击处理地雷布局，自己没有雷，自己周围也不会有雷
        if (!gameManager.ismineSetupFinished)
        {
            gameManager.ismineSetupFinished = true;
            boardManager.SetupMine(this);
        }
        currentAction = UserAction.leftclick;
        changeState();
        currentAction = UserAction.nothing;
    }

    /// <summary>
    /// 使用这种变通方式处理右键点击
    /// </summary>
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentAction = UserAction.rightclick;
            changeState();
            currentAction = UserAction.nothing;
        }
    }

    /// <summary>
    /// minebox状态机
    /// </summary>
    void changeState()
    {
        switch (currentState)
        {
            case BoxState.covered:
                if (currentAction == UserAction.leftclick)
                {
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
                //open不处理任何状态推移
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
