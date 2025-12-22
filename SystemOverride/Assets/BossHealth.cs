using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 1000;
    public int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public float GetHpNormalized()
    {
        return (float)currentHP / maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
}
