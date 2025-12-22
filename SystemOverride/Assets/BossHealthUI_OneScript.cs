using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class BossHealthUI_OneScript : MonoBehaviour
{
    [Header("HP (Integer)")]
    [SerializeField] private int maxHp = 1000;
    public int MaxHp => maxHp;
    public int CurrentHp { get; private set; }

    [Header("UI References (Top-Left HUD)")]
    [SerializeField] private Slider hpSlider;           // Min 0, Max 1 권장
    [SerializeField] private TextMeshProUGUI hpText;    // "current / max"

    [Header("Options")]
    [SerializeField] private bool enableTestKeys = false;
    [SerializeField] private int testDamage = 10;
    [SerializeField] private int testHeal = 10;

    [Header("EndingScene")]
    [SerializeField] private string endingSceneName = "EndingScene";
    [SerializeField] private float endingLoadDelay = 0f; // 연출용 딜레이(원하면 1~2초)

    public event Action OnDied;

    private bool isDead = false;

    private void Awake()
    {
        CurrentHp = Mathf.Clamp(maxHp, 1, int.MaxValue); // maxHp가 0 이하로 들어오는 실수 방지
        maxHp = CurrentHp;
        RefreshUI();
    }

    private void Update()
    {
        if (!enableTestKeys) return;

        // 테스트용: 3 = 데미지, 4 = 회복
        if (Input.GetKeyDown(KeyCode.Alpha3)) TakeDamage(testDamage);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Heal(testHeal);
    }

    // -------------------------
    // Public API (게임에서 호출)
    // -------------------------
    public void TakeDamage(int amount)
    {
        if (isDead) return;      // 이미 죽었으면 무시
        if (amount <= 0) return;

        int before = CurrentHp;
        CurrentHp = Mathf.Clamp(CurrentHp - amount, 0, maxHp);

        if (CurrentHp != before)
            RefreshUI();

        if (CurrentHp <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;      // 죽은 뒤 회복 방지(원하면 이 줄 삭제)
        if (amount <= 0) return;

        int before = CurrentHp;
        CurrentHp = Mathf.Clamp(CurrentHp + amount, 0, maxHp);

        if (CurrentHp != before)
            RefreshUI();
    }

    public void SetMaxHp(int newMax, bool fillToMax = true)
    {
        newMax = Mathf.Max(1, newMax);
        maxHp = newMax;

        if (fillToMax)
            CurrentHp = maxHp;
        else
            CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);

        RefreshUI();
    }

    // -------------------------
    // Death -> Ending
    // -------------------------
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        OnDied?.Invoke();

        if (endingLoadDelay > 0f)
            Invoke(nameof(LoadEndingScene), endingLoadDelay);
        else
            LoadEndingScene();
    }

    private void LoadEndingScene()
    {
        // endingSceneName이 비어있으면 현재 씬 유지 (방어)
        if (string.IsNullOrWhiteSpace(endingSceneName))
        {
            Debug.LogWarning("[BossHealthUI] endingSceneName is empty. Not loading scene.");
            return;
        }

        SceneManager.LoadScene(endingSceneName);
    }

    // -------------------------
    // Internal
    // -------------------------
    private void RefreshUI()
    {
        // UI가 연결 안 되어 있어도 게임이 터지지 않게 방어
        if (hpSlider != null)
            hpSlider.value = (maxHp <= 0) ? 0f : (float)CurrentHp / maxHp;

        if (hpText != null)
            hpText.text = $"{CurrentHp} / {maxHp}";
    }
}
