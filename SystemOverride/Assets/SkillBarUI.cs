using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarUI : MonoBehaviour
{
    [SerializeField] private PlayerSkillController player;
    [SerializeField] private Button[] buttons = new Button[4];

    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int idx = i;
            if (buttons[idx] != null)
                buttons[idx].onClick.AddListener(() => player.UseSkill(idx));
        }
    }
}
