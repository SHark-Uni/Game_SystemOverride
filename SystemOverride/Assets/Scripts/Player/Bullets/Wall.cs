using System.Collections;
using System.Collections.Generic;
using Scripts.Common;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    int hp;
    void Awake()
    {
        hp = 100;
    }
    public void TakeDamage(int atk, IAttacker attacker)
    {
        hp -= atk;
        Debug.Log(attacker.attackPower);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
