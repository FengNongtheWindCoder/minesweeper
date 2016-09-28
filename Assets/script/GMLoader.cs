/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     GMLoader.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-09-28
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

public class GMLoader : MonoBehaviour
{
    public GameObject gamemanager;
    public void Awake()
    {
        if(GameManager.instance == null)
        {
            Instantiate(gamemanager);
        }
    }
}
