using Scripts.Common;
using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Scripts.Player
{
    public class GrappleState : PlayerSuperState
    {
        RaycastHit2D _anchor;
        LineRenderer _ropeRender;
        DistanceJoint2D _playerJoint;
        bool IsAchored;
        public GrappleState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am) 
            : base(owner, stateMachine, name, rb, am)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            IsAchored = false;
            _playerJoint = _owner.joint;
            _ropeRender = _owner.rope;

            if (!TryHook())
            {
                EndHooking();
                _stateMachine.ChangeState(_owner.idleState);
            }
            return;
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (IsAchored && _inputAction.HookKeyboard.WasPerformedThisFrame())
            {
                EndHooking();
                _stateMachine.ChangeState(_owner.idleState);
                return;
            }

            if (_owner.playerInput.x != 0)
            {
                Swing();
            }

            if (_inputAction.Jump.WasPerformedThisFrame())
            {
                EndHooking();
                _stateMachine.ChangeState(_owner.jumpState);
                return;
            }

            UpdateLine();
        }

        public override void Exit()
        {
            base.Exit();

            return;
        }


        private bool TryHook()
        {
            Vector3 renderPos = _owner.currentMousePosition;
            renderPos.z = -10;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(renderPos);

            //점 RayCast
            _anchor = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, (int)eLayerMask.Ground);

            if (_anchor.collider != null)
            {
                Hooking();
                return true;
            }
            return false;
        }

        private void Hooking()
        {
            _playerJoint.connectedAnchor = _anchor.point;

            //오른쪽을 바라봤는데 왼쪽 클릭 or 왼쪽 바라봤는데 오른쪽 클릭
            if (needFlip())
            {
                _owner.Flip();
            }
            _playerJoint.distance = _owner.grappleLength;

            _playerJoint.enabled = true;
            IsAchored = true;


            DrawLine();
        }

        private void DrawLine()
        {
            _ropeRender.SetPosition(0, _anchor.point);
            _ropeRender.SetPosition(1, _owner.playerPosition);

            _ropeRender.enabled = true;
        }

        private void UpdateLine()
        {
            _ropeRender.SetPosition(1, _owner.playerPosition);
        }

        private void EndHooking()
        {
            _playerJoint.enabled = false;
            _ropeRender.enabled = false;
        }

        private bool needFlip()
        {
            return (_owner.facingDir == 1 && (_anchor.point.x < _owner.playerPosition.x))
                || (_owner.facingDir == -1 && (_anchor.point.x > _owner.playerPosition.x));
        }


        private void Swing()
        {
            //Anchor와 Player 방향 벡터의 수직벡터를 계산해서, Player의 입력방향을 곱해주는 방향으로 
            //지속적인 힘을 가해주기.
            Vector2 playerPos = _owner.playerPosition;
            Vector2 dir = (_anchor.point - playerPos).normalized;
            //360도 돌아가면 안되기 때문에, 각도가 특정방향 이상되면 더 이상 입력을 못받도록 해야하나?
            Vector2 swingDirection = new Vector2(dir.y, -dir.x);

            Vector2 achorToPlayerDir = (playerPos - _anchor.point);


            float degree = Mathf.Atan2(achorToPlayerDir.y, achorToPlayerDir.x) * Mathf.Rad2Deg;

            if (degree > -10f || degree < -170f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            }

            _rb.AddForce(swingDirection * _owner.playerInput.x * 5);
        }
    }
}

