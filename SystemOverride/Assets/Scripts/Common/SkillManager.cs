using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player;
using Scripts.Skill;
public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;
    public SkillDataBase _skillData;

    //스킬의 정보를 가져올 수 있음. UI
    Dictionary<ulong, SkillData> _skillDataStore;
    public static SkillManager instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            Init();
            return;
        }
        Destroy(this);
        return;
    }

    private void Init()
    {
        _skillDataStore = _skillData.GetDictionary();
    }

    public bool TryGetSkillBaseData(ulong id, out SkillData ret)
    {
        bool _isValid;
        _isValid = _skillDataStore.TryGetValue(id, out ret);
        if (_isValid)
        {
            return true;
        }
        return false;
    }



}
