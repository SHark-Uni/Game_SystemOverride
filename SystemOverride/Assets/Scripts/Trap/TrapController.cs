using Scripts.Common;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour, IAttacker
{
    public int damage = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌 감지됨");
        IDamageable player = other.GetComponent<IDamageable>();

        if (player != null)
        {
            // this를 넘겨서 공격자의 위치를 Player.TakeDamage에서 사용할 수 있게 함
            player.TakeDamage(damage, this);
            Debug.Log("데미지 받음");
        }
    }

    // IAttacker 구현 (간단한 구현)
    public int attackPower => damage;

    public void Attack(IDamageable target)
    {
        Debug.Log("TrapController Attack 호출됨");
        // 필요하면 즉시 데미지 적용하도록 구현할 수 있음
        target?.TakeDamage(damage, this);
    }

    public Vector3 GetAttackerPos()
    {
        return transform.position;
    }
}
