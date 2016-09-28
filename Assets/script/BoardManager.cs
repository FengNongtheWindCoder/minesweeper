/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     BoardManager.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-09-27
 *Description:   处理布局逻辑
 *History:  
**********************************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    private List<Vector2> allPostion = new List<Vector2>();
    private Dictionary<Vector2, MineboxController> allMineboxdict = new Dictionary<Vector2, MineboxController>();
    public GameObject MineboxPrefab;
    int currentSizeX = 0;
    int currentSizeY = 0;
    int currentMineNum = 0;
    public int safeboxNum = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 用来在指定范围内放置初始minebox
    /// </summary>
    /// <param name="size_x"></param>
    /// <param name="size_y"></param>
    /// <param name="mine_num"></param>
    public void SetupEmpty(int size_x, int size_y, int mine_num)
    {
        currentSizeX = size_x;
        currentSizeY = size_y;
        currentMineNum = mine_num;
        safeboxNum = size_x * size_y - mine_num;
        allPostion.Clear();
        allMineboxdict.Clear();
        for (int x = 0; x < currentSizeX; x++)
        {
            for (int y = 0; y < currentSizeY; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject mineboxGameobject = Instantiate(MineboxPrefab, pos, Quaternion.identity) as GameObject;
                MineboxController controller = mineboxGameobject.GetComponent<MineboxController>();
                controller.boardManager = this;
                allPostion.Add(pos);
                allMineboxdict.Add(pos, controller);
            }
        }
    }
    /// <summary>
    ///  用来设置地图里的雷，同时忽略输入的位置，输入的位置周围也不能有雷
    /// </summary>
    /// <param name="ignoreBox"></param>
    public void SetupMine(MineboxController ignoreBox)
    {
        removeIgnoreboxs(ignoreBox);
        for (int i = 0; i < currentMineNum; i++)
        {
            MineboxController mineBox = Randombox();
            mineBox.HasMine = true;
            List<Vector2> neighbours = getNeighbourBoxs(mineBox);
            foreach (var boxpos in neighbours)
            {
                allMineboxdict[boxpos].surroundingMine++;
            }
        }
    }
    /// <summary>
    ///  删除布局数组里的某个特定位置，及周围位置，他们不会包含地雷
    /// </summary>
    /// <param name="ignoreBox"></param>
    void removeIgnoreboxs(MineboxController ignoreBox)
    {
        Vector2 curpos = ignoreBox.transform.position;
        allPostion.Remove(curpos);
        List<Vector2> neighbours = getNeighbourBoxs(ignoreBox);
        foreach (var box in neighbours)
        {
            allPostion.Remove(box);
        }
    }
    /// <summary>
    /// 获取指定位置周围位置的box
    /// </summary>
    /// <param name="mineBox"></param>
    /// <returns></returns>
    List<Vector2> getNeighbourBoxs(MineboxController mineBox)
    {
        List<Vector2> result = new List<Vector2>();
        Vector3 curpos = mineBox.transform.position;
        for (int offset_x = -1; offset_x < 2; offset_x++)
        {
            for (int offset_y = -1; offset_y < 2; offset_y++)
            {
                Vector2 pos = new Vector2(curpos.x + offset_x, curpos.y + offset_y);
                if (allMineboxdict.ContainsKey(pos))
                {
                    result.Add(pos);
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 在位置数组里取随机的一个位置，并获取对应的minebox，删除数组内的这个位置
    /// </summary>
    /// <returns></returns>
    MineboxController Randombox()
    {
        int randomIndex = Random.Range(0, allPostion.Count);
        Vector2 randomboxpos = allPostion[randomIndex];
        allPostion.RemoveAt(randomIndex);
        return allMineboxdict[randomboxpos];
    }
}
