using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    // Start is called before the first frame update
    public GameObject _OptionBtnPrefab;
    public GameObject _HpBarPrefab;
    public GameObject _OptionPannel;

    private GameObject _OptionBtn;
    private GameObject _HpBar;
    private GameObject _OptionMenu;

    public Image _hpScrollBar;

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
        _OptionBtn = Instantiate(_OptionBtnPrefab, Vector3.zero, Quaternion.identity);
        _HpBar = Instantiate(_HpBarPrefab, Vector3.zero, Quaternion.identity);
        _OptionMenu = Instantiate(_OptionPannel, Vector3.zero, Quaternion.identity);

        _hpScrollBar = _HpBar.GetComponentInChildren<Image>();

        DontDestroyOnLoad(_OptionBtn);
        DontDestroyOnLoad(_HpBar);
        DontDestroyOnLoad(_OptionMenu);
        DontDestroyOnLoad(_hpScrollBar);
    }

    public void SetMainUI()
    {
        _OptionBtn.SetActive(true);
        _HpBar.SetActive(true);
    }

    public void OffMainUI()
    {
        _OptionBtn.SetActive(false);
        _HpBar.SetActive(false);
    }

    public void SetHp(float hpRate)
    {
        _hpScrollBar.fillAmount = hpRate;
    }

    public void TurnOnOptionPanel()
    {
        _OptionMenu.SetActive(true);
    }

    public void TurnOffOptionPanel()
    {
        _OptionMenu.SetActive(false);
    }

}
