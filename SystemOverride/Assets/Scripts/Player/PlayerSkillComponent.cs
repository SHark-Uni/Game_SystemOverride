using Scripts.Common;
using Scripts.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerSkillComponent
    {
        Player _player;
        public PlayerSkillComponent(Player player)
        {
            _player = player;

            _mySkillList = new Dictionary<ulong, SkillState>();
            InitSkillList();
        }

        private Dictionary<ulong, SkillState> _mySkillList;

        private void InitSkillList()
        {
            SkillManager sm = SkillManager.instance;

            bool ret;
            for (ulong i = 0; i < (ulong)eSkillId.Skill_IDCount; i++)
            {
                ret = sm.TryGetSkillBaseData(i, out SkillData data);
                if (ret == false)
                {
                    continue;
                }

                _mySkillList[i] = new SkillState(i, data._cooldown, data._elapsedTime);
            }
        }

        public void InvisibleSkillProcess()
        {
            //5초간 벽통과 상태 만들기, 쿨다운 30초
            //Validate
            SkillState state = _mySkillList[(ulong)eSkillId.Invisible];

            if (state._lastActive + state._cooldown <= Time.time)
            {
                state._CanCasting = true;
            }

            if (!state._CanCasting)
            {
                return;
            }

            state.SetUpdateTime(Time.time);
            state._CanCasting = false;

            InvisiblePlayer buff = new InvisiblePlayer(
                data: new BuffState(state._id, state._elapsedTime),
                caster: _player);

            _player.buffManager.InsertBuff(buff);
        }
        public void ImmotalSKillProcess()
        {
            SkillState state = _mySkillList[(ulong)eSkillId.Immotal];
            if (state._lastActive + state._cooldown <= Time.time)
            {
                state._CanCasting = true;
            }

            if (!state._CanCasting)
            {
                return;
            }

            state.SetUpdateTime(Time.time);
            state._CanCasting = false;
            InvisiblePlayer buff = new InvisiblePlayer(
                data: new BuffState(state._id, state._elapsedTime),
                caster: _player);

            _player.buffManager.InsertBuff(buff);
        }

        public void LaserBlasterProcess()
        {
            SkillState state = _mySkillList[(ulong)eSkillId.LaserBuster];

            if (state._lastActive + state._cooldown <= Time.time)
            {
                state._CanCasting = true;
            }

            if (!state._CanCasting)
            {
                return;
            }

            state.SetUpdateTime(Time.time);
            state._CanCasting = false;
            LaserBlasterPlayer buff = new LaserBlasterPlayer(
                data: new BuffState(state._id, state._elapsedTime),
                caster: _player);

            _player.buffManager.InsertBuff(buff);
        }
        public void prepareHackBullet()
        {
            SkillState state = _mySkillList[(ulong)eSkillId.HackBullet];

            if (!state._CanCasting)
            {
                return;
            }

            state.SetUpdateTime(Time.time);
            state._CanCasting = false;
            HackBulletPlayer buff = new HackBulletPlayer(
                data: new BuffState(state._id, state._elapsedTime, true, 1),
                caster: _player
                );

            _player.buffManager.InsertBuff(buff);
        }

    }

}
