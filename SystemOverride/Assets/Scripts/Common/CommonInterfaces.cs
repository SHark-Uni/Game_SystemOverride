using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Common
{
    public interface IHitable
    {
        void TakeDamage(int atk);
    }

    public interface IDamageable
    {
        void TakeDamage(int atk, IAttacker attacker);
    }

    public interface IAttacker
    {
        int attackPower { get; }
        void Attack(IDamageable target);
        Vector3 GetAttackerPos();
    }

    public interface IPoolable
    {
        void OnAlloc();
        void OnRelease();
    }
}

