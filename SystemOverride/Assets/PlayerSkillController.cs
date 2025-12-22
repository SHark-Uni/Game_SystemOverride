using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("Slots (0~3)")]
    public SkillData[] slots = new SkillData[4];

    private float[] nextReadyTime = new float[4];

    public void UseSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;

        var skill = slots[slotIndex];
        if (skill == null)
        {
            Debug.LogWarning($"Skill slot {slotIndex + 1} is empty.");
            return;
        }

        if (Time.time < nextReadyTime[slotIndex])
        {
            float remain = nextReadyTime[slotIndex] - Time.time;
            Debug.Log($"Skill {skill.skillName} cooldown: {remain:0.0}s");
            return;
        }

        // 쿨타임 갱신 후 시전
        nextReadyTime[slotIndex] = Time.time + skill.cooldown;
        skill.Cast(gameObject);
    }
}
