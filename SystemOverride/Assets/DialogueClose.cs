using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueClose : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;

    public void Close()
    {
        dialoguePanel.SetActive(false);
    }
}
