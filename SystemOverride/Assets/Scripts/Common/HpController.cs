using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    // Start is called before the first frame update
    private Image _Hpbar;
    private void Awake()
    {
        _Hpbar = GetComponentInChildren<Image>();
        Debug.Log(_Hpbar.gameObject.name);
    }
    private void Start()
    {
        
    }
    public void SetHp(float fillAmount)
    {
        _Hpbar.fillAmount = fillAmount;
    }
}
