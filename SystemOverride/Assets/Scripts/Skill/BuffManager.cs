using Scipts.Skill;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    private List<Buff> _BuffList;
    private Player_Temp _owner;
    public BuffManager(Player_Temp owner)
    {
        _owner = owner;
        _BuffList = new List<Buff>();
    }
    public void UpdateBuff()
    {
        for (int i = _BuffList.Count - 1; i >= 0; i--)
        {
            if (_BuffList[i].IsInitial())
            {
                _BuffList[i].OnActive();
                _BuffList[i].SetIsNotInit();
            }

            if (_BuffList[i].IsExpired())
            {
                _BuffList[i].OnUnActive();
                _BuffList.RemoveAt(i);
                continue;
            }
            _BuffList[i].OnUpdate();
        }
    }


    public bool TryFindCountedBaseSkill(ulong skillId)
    {
        for (int i = 0; i < _BuffList.Count; i++)
        {
            if (_BuffList[i].IsCountBaseSkill() == false)
            {
                continue;
            }

            if (_BuffList[i].IsEqualToId(skillId))
            {
                return true;
            }
        }
        return false;
    }

    public Buff FindCountBaseSkillOrNull(ulong skillId)
    {
        for (int i = 0; i < _BuffList.Count; i++)
        {
            if (_BuffList[i].IsCountBaseSkill() == false)
            {
                continue;
            }

            if (_BuffList[i].IsEqualToId(skillId))
            {
                return _BuffList[i];
            }
        }
        return null;
    }


    public void InsertBuff(Buff buff)
    {
        if (buff.IsCountBaseSkill())
        {
            //중복된 스킬이 이미 있다면, Buff에 등록하지 않는다.
            //이건 정책에 따라서, 카운팅을 갱신해도될듯.
            if (TryFindCountedBaseSkill(buff.GetId()) == true)
            {
                return;
            }
        }
        _BuffList.Add(buff);
        return;
    }

}
