using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackDashState : MonoBehaviour
{
    private SpriteRenderer _playerSpriteRenderer;
    private PlayerMove _playerMove;
    public Rigidbody2D _rb;
    public Animator _dashAnim;
    public Player _player;

    // 대시 관련
    private bool _canDash = true;
    private bool _isDashing;
    private bool _isRun = false;
    private float _dashingPower = 7f;
    private float _dashingTime = 0.5f;
    private float _dashingCooldown = 1f;

    private void Awake()
    {
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMove = new PlayerMove();
        _dashAnim = GetComponent<Animator>();
        _player = GetComponent<Player>();
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

    private IEnumerator DashAndBackDash()
    {
        Vector2 dir = _player.transform.position;

        // canDash(쿨타임 계산용), isDash(동작 상태 관리용)을 초기화
        _canDash = false;
        _isDashing = true;

        // 대쉬중에는 Y축이 변하지 않게 하기 위해 중력을 제거한다.
        float originalGfavity = _rb.gravityScale;

        // 그 전에 원래의 중력(GravityScale) 값을 변수에 저장한다.
        _rb.gravityScale = 0f;

        // 가만히 서있을 때의 대쉬 방향을 정해줌
        if (dir.x == 0) dir = Vector2.right;

        // 속력을 수정, 그리고 dashingTime 후에 종료
        // Vector값을 줘서 플레이어의 위치값을 힘의 값으로 받아버림 > 플레이어 위치의 절대값이 클 수록 힘이 세짐
        if (!_isRun) // 달리기 or 걷기 상태가 아닐 때
        {
            if (_playerSpriteRenderer.flipX == false) // 오른쪽 바라볼 때
            {
                if (_playerMove.Player.Dash.IsPressed())
                {
                    _rb.velocity = new Vector2(_dashingPower, 0f); // 오른쪽 백대시
                }
            }
            else // 왼쪽 바라볼 때
            {
                if (_playerMove.Player.Dash.IsPressed())
                {
                    _rb.velocity = new Vector2(-_dashingPower, 0f); // 왼쪽 백대시
                }
            }
        }
        else // 달리기 or 걷기 상태일 때
        {

            if (_playerSpriteRenderer.flipX == false) // 오른쪽 바라볼 때
            {
                if (_playerMove.Player.Dash.IsPressed())
                {
                    _rb.AddForce(new Vector2(_dashingPower * 10, 0f), ForceMode2D.Impulse); // 오른쪽 백대시
                }
            }
            else // 왼쪽 바라볼 때
            {
                if (_playerMove.Player.Dash.IsPressed())
                {
                    _rb.AddForce(new Vector2(-_dashingPower * 10, 0f), ForceMode2D.Impulse); // 왼쪽 백대시
                }
            }
        }

        _dashAnim.SetBool("isDash", true); // 대시 애니매이션 실행
        yield return new WaitForSeconds(_dashingTime);

        // player의 중력을 돌려 놓고 isDashing을 false
        _rb.gravityScale = originalGfavity;
        _isDashing = false;

        // 단, 쿨타임 계산을 위해 canDash는 new WaitForSeconds(dashingCooldown); 를 통해 dashingCooldown 후에 true로 초기화
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;

        // 대시 애니메이션 종료
        _dashAnim.SetBool("isDash", false);
    }

    void OnDash()
    {
        if ((_playerMove.Player.Dash.IsPressed() || _playerMove.Player.BackDash.IsPressed()) && _canDash)
        {
            StartCoroutine(DashAndBackDash());
        }
    }

    void Upddate()
    {
        OnDash();
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
