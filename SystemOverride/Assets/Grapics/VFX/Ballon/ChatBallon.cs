using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBallon : MonoBehaviour
{
    public Image _Ballon;
    public TextMeshProUGUI _Text;

    private void Awake()
    {
        _Ballon = GetComponentInChildren<Image>();
        _Text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        
    }

    public void SetText(string text,float fontSize)
    {
        _Text.fontSize = fontSize;
        _Text.text = text;
    }
}
