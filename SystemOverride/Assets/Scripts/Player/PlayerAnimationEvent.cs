using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class PlayerAnimationEvent : MonoBehaviour
    {
        Player _player;

        private void Start()
        {
            _player = GetComponentInParent<Player>();
        }

        public void OnAttackEnd()
        {
            _player.SetAnimTrigger();
        }
    }
}

