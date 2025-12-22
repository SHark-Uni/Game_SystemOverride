using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject audioPanel;
    //[SerializeField] private GameObject videoPanel;

    private void Start()
    {
        OpenAudio();
        //OpenVideo();
    }


    public void OpenAudio()
    {
        audioPanel.SetActive(true);
        //videoPanel.SetActive(true);
    }

    //public void OpenVideo()
    //{
    //    audioPanel.SetActive(true);
    //    videoPanel.SetActive(true);
    //}

}
