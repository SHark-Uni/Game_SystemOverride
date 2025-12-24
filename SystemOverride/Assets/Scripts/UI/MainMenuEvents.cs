using Scripts.Common;
using Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour
{
    public void OnClickGameStart()
    {
        LoadingManager.instance.ChangeSceneWithLoadingPanel(eSceneType._GameScene, transform.position, OnEnterGameScene);
    }

    public void OnClickOptionBtn()
    {
        UIManager.instance.TurnOnOptionPanel();
        
    }

    public void OnReturnToMainScene()
    {
        SceneLoader.instance.LoadScene(eSceneType._Main);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    private void OnEnterGameScene()
    {
        SoundManager.instance.ChangeBGM(eSceneType._GameScene.ToString());
        UIManager.instance.SetMainUI();
    }



}
