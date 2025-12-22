using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string bgmParam = "BGMVolume";
    [SerializeField] private string sfxParam = "SFXVolume";

    [Header("UI (0~1)")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private const string KEY_BGM = "Opt_BGM";
    private const string KEY_SFX = "Opt_SFX";

    private void Start()
    {
        // 저장값 불러오기(없으면 1.0)
        float bgm = PlayerPrefs.GetFloat(KEY_BGM, 1f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);

        bgmSlider.SetValueWithoutNotify(bgm);
        sfxSlider.SetValueWithoutNotify(sfx);

        ApplyBgm(bgm);
        ApplySfx(sfx);

        bgmSlider.onValueChanged.AddListener(ApplyBgm);
        sfxSlider.onValueChanged.AddListener(ApplySfx);
    }

    private void ApplyBgm(float value01)
    {
        mixer.SetFloat(bgmParam, LinearToDb(value01));
        PlayerPrefs.SetFloat(KEY_BGM, value01);
        PlayerPrefs.Save();
    }

    private void ApplySfx(float value01)
    {
        mixer.SetFloat(sfxParam, LinearToDb(value01));
        PlayerPrefs.SetFloat(KEY_SFX, value01);
        PlayerPrefs.Save();
    }

    // 0이면 -80dB(사실상 무음), 1이면 0dB
    private float LinearToDb(float value01)
    {
        if (value01 <= 0.0001f) return -80f;
        return Mathf.Log10(value01) * 20f;
    }
}
