using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    // Start is called before the first frame update
    public GameObject _MainUI;
    private HpController controller;
    public Image _HpBar;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
            return;
        }
        Destroy(this);
        return;
    }

    private void Init()
    {
        controller = _MainUI.GetComponentInChildren<HpController>();
    }

    public void SetMainUI()
    {
        _MainUI.SetActive(true);
    }

    public void SetHp(float hpRate)
    {
        if (controller == null)
        {
            Debug.Log("controller is null");
        }
        //Debug.Log(controller.gameObject.name);
        Image soure = controller.GetComponentInChildren<Image>();
        //Debug.Log(soure.gameObject.name);

        Debug.Log(hpRate);
        _HpBar.fillAmount = hpRate;
        //controller.SetHp(hpRate);
    }
}
