using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private Resolution[] resolutions;

    private const string KEY_RES = "Opt_ResIndex";
    private const string KEY_FULL = "Opt_Fullscreen";
    private const string KEY_QUAL = "Opt_Quality";

    private void Start()
    {
        // 🔒 널 가드 (이제 통과해야 정상)
        if (resolutionDropdown == null || fullscreenToggle == null || qualityDropdown == null)
        {
            Debug.LogError("[VideoSettingsUI] TMP_Dropdown / Toggle 연결 안 됨");
            enabled = false;
            return;
        }

        // 이하 로직 동일
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRateRatio.value:0.#}Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedResIndex = PlayerPrefs.GetInt(KEY_RES, currentIndex);
        savedResIndex = Mathf.Clamp(savedResIndex, 0, resolutions.Length - 1);
        resolutionDropdown.SetValueWithoutNotify(savedResIndex);

        resolutionDropdown.onValueChanged.AddListener(ApplyResolution);

        bool savedFull = PlayerPrefs.GetInt(KEY_FULL, Screen.fullScreen ? 1 : 0) == 1;
        fullscreenToggle.SetIsOnWithoutNotify(savedFull);
        fullscreenToggle.onValueChanged.AddListener(ApplyFullscreen);

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));

        int savedQual = PlayerPrefs.GetInt(KEY_QUAL, QualitySettings.GetQualityLevel());
        savedQual = Mathf.Clamp(savedQual, 0, QualitySettings.names.Length - 1);
        qualityDropdown.SetValueWithoutNotify(savedQual);
        qualityDropdown.onValueChanged.AddListener(ApplyQuality);
    }

    private void ApplyResolution(int index)
    {
        var r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRateRatio);
        PlayerPrefs.SetInt(KEY_RES, index);
    }

    private void ApplyFullscreen(bool isFull)
    {
        Screen.fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetInt(KEY_FULL, isFull ? 1 : 0);
    }

    private void ApplyQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt(KEY_QUAL, index);
    }
}
