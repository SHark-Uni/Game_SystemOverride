using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SitState : MonoBehaviour
{
    private BoxCollider2D _boxCol;
    private PlayerMove _playerMove;
    public Animator _dashAnim;

    private void Awake()
    {
        _playerMove = new PlayerMove();
        _dashAnim = GetComponent<Animator>();
        _boxCol = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        // 입력 맵을 한 번만 활성화
        _playerMove?.Player.Enable();
    }

    private void OnDisable()
    {
        // 비활성화
        _playerMove?.Player.Disable();
    }

    void OnSitDown()
    {
        if (_playerMove.Player.SitDown.WasPressedThisFrame())
        {
            _dashAnim.SetBool("isSitDown", true);
            _boxCol.size = new Vector2(0.3f, 0.25f);
            _boxCol.offset = new Vector2(0f, -0.2f);

        }
        else if (_playerMove.Player.SitDown.WasReleasedThisFrame())
        {
            _dashAnim.SetBool("isSitDown", false);
            _boxCol.size = new Vector2(0.3f, 0.5f);
            _boxCol.offset = new Vector2(0f, -0.1f);
        }
    }

    void Update()
    {
        OnSitDown();
    }
    /**
   virtual void OnEnter()
   {

   }

   virtual void OnUpdate()
   {

   }

   virtual void OnExit()
   {

   }
   **/

}
