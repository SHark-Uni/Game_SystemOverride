using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorController : MonoBehaviour
{
    public GameObject floor1_Right;
    public GameObject floor2;
    public GameObject floor2_Right;
    public GameObject floor3;
    public GameObject floor3_Right;
    public GameObject player;

    Vector2 _playerPos;
    Vector2 _floor1Pos;
    Vector2 _floor2Pos;
    Vector2 _floor3Pos;
    Vector2 _floor1_Right_Pos;
    Vector2 _floor2_Right_Pos;
    Vector2 _floor3_Right_Pos;

    void Start()
    {
        _playerPos = player.transform.position;
        _floor1_Right_Pos = floor1_Right.transform.position;
        _floor2Pos = floor2.transform.position;
        _floor2_Right_Pos = floor2_Right.transform.position;
        _floor3Pos = floor3.transform.position;
        _floor3_Right_Pos = floor3_Right.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _playerPos == _floor1_Right_Pos)
        {
            _playerPos = _floor2Pos;
        }
        else if (Input.GetKeyDown(KeyCode.F) && _playerPos == _floor2_Right_Pos)
        {
            _playerPos = _floor3Pos;
        }
        else if (Input.GetKeyDown(KeyCode.F) && _playerPos == _floor3_Right_Pos)
        {
            SceneManager.LoadScene("_Boss");
        }
    }
}
