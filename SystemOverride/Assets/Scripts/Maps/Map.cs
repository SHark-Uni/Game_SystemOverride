using Scripts.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public SpawnListSO _location;
    private void Awake()
    {
        
    }

    private void Start()
    {
        List<Vector2> spawnList = _location._SpawnLocations;
        for (int i = 0; i < spawnList.Count; i++)
        {
            MonsterSpawner.instance.SpawnMonsterAt(spawnList[i], Quaternion.identity);
        }   
    }
}
