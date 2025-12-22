using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common;

namespace Scripts.Skill
{
    using Scripts.Common;
    [CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
    public class SkillDataBase : ScriptableObject
    {
        [SerializeField] private List<SkillData> _List;

        public Dictionary<ulong, SkillData> GetDictionary()
        {
            Dictionary<ulong, SkillData> ret = new Dictionary<ulong, SkillData>();

            foreach (SkillData data in _List)
            {
                if (ret.ContainsKey(data._id))
                {
                    Debug.Log("스킬코드가 중복됩니다.");
                    break;
                }
                ret[data._id] = data;
            }

            return ret;
        }
    }

}
