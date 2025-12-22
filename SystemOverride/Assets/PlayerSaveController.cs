using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveController : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void SaveGame()
    {
        SaveManager.SavePlayer(
            transform.position,
            playerHealth.currentHP
        );

        Debug.Log("Game Saved!");
    }
}
