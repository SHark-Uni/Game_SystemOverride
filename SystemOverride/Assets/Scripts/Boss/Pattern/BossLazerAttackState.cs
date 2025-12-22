using Scripts.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.BossStateMachine;
using scipts.Boss;

namespace Scipts.Boss
{
    public class BossLazerAttackState : BossSuperState
    {
        private float _timer;
        private int _phase;

        private List<LaserTurret> _activeTurrets = new List<LaserTurret>();

        public BossLazerAttackState(Boss_Temp _boss, BossStateMachine<Boss_Temp> _stateMachine, Rigidbody2D _bossrb, Animator _bossam) : base(_boss, _stateMachine, "IsLazer", _bossrb, _bossam)
        {
        }


        public override void Enter()
        {
            base.Enter();
            _timer = 0f;
            _phase = 0;
            _activeTurrets.Clear();

            Transform target = GameObject.FindGameObjectWithTag("Player").transform;

            // 1. 천장 위치들에 터렛 소환
            if (_bossOwner._ceilingSpawnPoints != null)
            {
                foreach (Transform spawnPoint in _bossOwner._ceilingSpawnPoints)
                {
                    
                    GameObject obj = GameObject.Instantiate(_bossOwner._turretPrefab, spawnPoint.position, Quaternion.identity);

                   
                    LaserTurret turret = obj.GetComponent<LaserTurret>();

                    if (turret != null)
                    {
                        // 터렛 초기화 (타겟, 회전속도 전달)
                        turret.Init(target, _bossOwner._turretTurnSpeed);

                        // 관리 리스트에 등록 (나중에 명령 내리기 위해)
                        _activeTurrets.Add(turret);
                    }
                }
            }
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _timer += Time.deltaTime;
            
            
        }

    }
}