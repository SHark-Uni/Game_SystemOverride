using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public string skillName = "Skill";
    public Sprite icon;
    public float cooldown = 1f;

    // 스킬 실행
    public abstract void Cast(GameObject caster);
}

