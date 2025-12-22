using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image hpFillImage;

    private void Update()
    {
        if (playerHealth == null) return;

        hpFillImage.fillAmount = playerHealth.GetHpNormalized();
    }
}
