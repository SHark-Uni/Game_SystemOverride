using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;

    [Header("Option")]
    [SerializeField] private bool triggerOnce = true;

    [Header("Auto Hide")]
    [SerializeField] private float autoHideSeconds = 5f;

    private bool triggered;
    private Coroutine hideRoutine;

    private void Awake()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false); // 시작은 숨김(원하면 삭제)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnce && triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // 패널 켜기
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // 이전에 예약된 숨김이 있으면 취소 후 다시 예약
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(AutoHide());
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(autoHideSeconds);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        hideRoutine = null;
    }
}
