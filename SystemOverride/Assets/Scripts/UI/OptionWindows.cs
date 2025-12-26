using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Scripts.Common;

public class OptionWindows : MonoBehaviour
{
    private Slider BGMvolumn;
    private Slider SFXvolumn;
    [SerializeField] private InputActionReference _input;

    private void Awake()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();

        BGMvolumn = sliders[0];
        SFXvolumn = sliders[1];
    }

    private void OnEnable()
    {
        _input.action.performed += OnCancelInput;
        _input.action.Enable();
    }
    private void OnDisable()
    {
        _input.action.performed -= OnCancelInput;
        _input.action.Disable();
    }

    private void Start()
    {
        BGMvolumn.onValueChanged.AddListener(OnBgmVolEdit);
        SFXvolumn.onValueChanged.AddListener(OnSFXVolEdit);
    }

    private void OnCancelInput(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    private void OnBgmVolEdit(float value)
    {
        SoundManager.instance.SetBGMVolume(value);
    }
    private void OnSFXVolEdit(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }
}
