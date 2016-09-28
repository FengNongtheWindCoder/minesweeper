/*********************************************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     ReloadScene.cs
 *Author:       DefaultCompany
 *Version:      1.0
 *UnityVersion：5.4.1f1
 *Date:         2016-09-28
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReloadScene : MonoBehaviour {

    /// <summary>
    /// Reloads the level.
    /// </summary>
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
