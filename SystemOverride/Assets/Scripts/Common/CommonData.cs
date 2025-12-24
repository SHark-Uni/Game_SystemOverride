using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Common
{
	public enum eLayerMask
	{
		Player = 1 << 6,
		Monster = 1 << 7,
		Ground = 1 << 8,
		Wall = 1 << 9,
		Hook = 1 << 10,
		Ghost = 1 << 11,
	}

	public enum eBulletType
	{ 
		Normal = 0,
		Hacking = 1,

		BulletTypeCount,
	}

	public enum eLayerNumber
	{
		Player = 6,
		Monster = 7,
		Ground = 8,
        Wall = 9,
		Hook = 10,
		Ghost = 11,
    }

	public enum eVFXId
	{ 
		onHitVFX = 0,
        laserVFX = 1,
	}

    public enum eSkillId : ulong
	{ 
		HackBullet = 0,
		Invisible,
		Immotal,
		LaserBuster,
		Skill_IDCount,
	}

	public enum eSkillBitMask : ulong
	{
        HackBullet = 1,
        Invisible = 1 << 1,
        Immotal = 1 << 2,
        LaserBuster = 1 << 3,
    }

	[System.Serializable]
	public struct PlayerStat
	{
		public PlayerStat(int maxHp,int hp, int atk, int def)
		{
			_maxHp = maxHp;
			_hp = hp;
			_atk = atk;
			_def = def;
		}
		public int _maxHp;
		public int _hp;
		public int _atk;
		public int _def;
	}

    [System.Serializable]
    public struct SkillData
    {
        public ulong _id;

        public string _name;
        public Sprite _icons;

        public bool _isCounted; 
        public int _counting; // isCounted가 false인 경우 0

        public float _cooldown;
        public float _elapsedTime; //지속시간 즉발형인 경우 0.
    }

	//플레이어가 소유한 스킬관리 대상. 쿨타임 관리.
	public class SkillState
	{
		public SkillState(ulong id, float cooldown, float elapsedTime)
		{
			_id = id;
			_cooldown = cooldown;
			_elapsedTime = elapsedTime;

            _CanCasting = true;
			_lastActive = 0.0f;
        }
        public ulong _id;

        public bool _isCounted;
        public int _counting;

        public float _cooldown;
        public float _elapsedTime;

		public void SetUpdateTime(float time)
		{
			_lastActive = time;
        }
		//

        public float _lastActive; //캐스팅한 시간
		public bool _CanCasting; 
    }

	public struct BuffState
	{
		public BuffState(ulong id, float elapsedTime, bool isCounted = false, int counting = 0)
		{
			_id = id;
			_elapsedTime = elapsedTime;

			_IsInitial = true;
			_IsExpired = false;

            _isCounted = isCounted;
            _counting = counting;
            _lastActive = Time.time;
		}

        public ulong _id;
		public bool _IsInitial; //스킬 쓰고나서 첫 프레임인가?

		public bool _isCounted; //카운팅 기반 버프스킬
		public int _counting; //카운트 기반 스킬이라면, 횟수에 기반함.

		public float _elapsedTime; //지속시간

		public float _lastActive; //캐스팅한 시간
		public bool _IsExpired; //버프가 꺼져야함 
    }

	public struct PlayerDashData
	{
        public float _dashForce;
        public float _dashDuration;
		public float _dashCooldown;
    }



}
