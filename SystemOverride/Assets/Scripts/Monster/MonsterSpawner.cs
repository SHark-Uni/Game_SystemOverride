using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        public static MonsterSpawner instance { get; private set; }
        [SerializeField] private Monster _monsterPrefeb;
        private ObjectPool<Monster> _monsterPool;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Init();
                DontDestroyOnLoad(this);
            }
            Destroy(this);
            return;
        }

        private void Init()
        {
            _monsterPool = new ObjectPool<Monster>();
            _monsterPool.Init(ConfigManager.MonsterPoolSize, _monsterPrefeb);
        }

        //위치를 기반으로 스폰 
        //스폰 포인트를 정하는 방식 ?
        public void SpawnMonsterAt(Vector2 pos, Quaternion rotate)
        {
            Monster mos = _monsterPool.alloc(pos, rotate);
            mos.gameObject.SetActive(true);
            return;
        }

        public void ReleasMonster(Monster monster)
        {
            monster.gameObject.SetActive(false);
            _monsterPool.release(monster);
            return;
        }
    }
}

