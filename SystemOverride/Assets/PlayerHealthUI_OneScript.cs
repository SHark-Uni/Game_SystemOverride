using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealthUI_OneScript : MonoBehaviour
{
    [Header("HP (Integer)")]
    [SerializeField] private int maxHp = 100;
    public int MaxHp => maxHp;
    public int CurrentHp { get; private set; }

    [Header("UI References (Top-Left HUD)")]
    [SerializeField] private Slider hpSlider;           // Min 0, Max 1 권장
    [SerializeField] private TextMeshProUGUI hpText;    // "current / max"

    [Header("Options")]
    [SerializeField] private bool enableTestKeys = false;
    [SerializeField] private int testDamage = 10;
    [SerializeField] private int testHeal = 10;

    [Header("OverScene")]
    [SerializeField] private string overSceneName = "OverScene";
    [SerializeField] private float overLoadDelay = 0f; // 연출용 딜레이(원하면 1~2초)
    private bool isDead;
    private float endingLoadDelay;

    public event Action OnDied;

    private void Awake()
    {
        CurrentHp = Mathf.Clamp(maxHp, 1, int.MaxValue); // maxHp가 0 이하로 들어오는 실수 방지
        maxHp = CurrentHp;
        RefreshUI();
    }

    private void Update()
    {
        if (!enableTestKeys) return;

        // 테스트용: 1 = 데미지, 2 = 회복
        if (Input.GetKeyDown(KeyCode.Alpha1)) TakeDamage(testDamage);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Heal(testHeal);
    }

    // -------------------------
    // Public API (게임에서 호출)
    // -------------------------
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        int before = CurrentHp;
        CurrentHp = Mathf.Clamp(CurrentHp - amount, 0, maxHp);

        if (CurrentHp != before)
            RefreshUI();

        if (CurrentHp <= 0)
            OnDied?.Invoke();

        if (CurrentHp <= 0)
            Die();
    }

    public void Heal(int amount)
    {
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
    // Death -> OverScene
    // -------------------------
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        OnDied?.Invoke();

        if (endingLoadDelay > 0f)
            Invoke(nameof(LoadOverScene), overLoadDelay);
        else
            LoadOverScene();
    }

    private void LoadOverScene()
    {
        // endingSceneName이 비어있으면 현재 씬 유지 (방어)
        if (string.IsNullOrWhiteSpace(overSceneName))
        {
            Debug.LogWarning("[BossHealthUI] overSceneName is empty. Not loading scene.");
            return;
        }

        SceneManager.LoadScene(overSceneName);
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