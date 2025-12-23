using Scripts.Common;
using Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour
{
    public void OnClickGameStart()
    {
        LoadingManager.instance.ChangeSceneWithLoadingPanel(eSceneType._GameScene, transform.position);
        
    }


    public void OnClickOption()
    { 
        //옵션 창 키기

    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
