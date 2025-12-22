using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [Header("Optional UI")]
    [SerializeField] private Image buttonIcon;          // 버튼 아이콘 Image (선택)
    [SerializeField] private Sprite soundOnSprite;      // 스피커 ON 스프라이트 (선택)
    [SerializeField] private Sprite soundOffSprite;     // 스피커 OFF 스프라이트 (선택)

    private const string KEY_MUTED = "Muted";           // 0 or 1

    private void Start()
    {
        // 저장된 설정 적용
        bool muted = PlayerPrefs.GetInt(KEY_MUTED, 0) == 1;
        ApplyMute(muted);
    }

    // 버튼 OnClick에 연결할 함수
    public void ToggleSound()
    {
        bool muted = !AudioListener.pause; // pause=true면 음소거 상태
        ApplyMute(muted);

        PlayerPrefs.SetInt(KEY_MUTED, muted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyMute(bool muted)
    {
        AudioListener.pause = muted;

        // 아이콘 바꾸고 싶으면(선택)
        if (buttonIcon != null && soundOnSprite != null && soundOffSprite != null)
        {
            buttonIcon.sprite = muted ? soundOffSprite : soundOnSprite;
        }
    }
}
