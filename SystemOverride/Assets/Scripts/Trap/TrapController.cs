using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player;

public class TrapController : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
