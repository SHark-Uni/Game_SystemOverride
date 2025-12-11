using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _playerSprite;

    private SpriteRenderer _playerSpriteRenderer;
    private PlayerMove _playerMove;
    public Rigidbody2D _rb;
    public Animator _dashAnim;

    private void Awake()
    {
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMove = new PlayerMove();
        _dashAnim  = GetComponent<Animator>();
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

    void OnDash()
    {
        if (_playerMove.Player.Dash.IsPressed())
        {
            if(_playerMove.Player.RightMove.IsPressed())
            {
                _playerSpriteRenderer.flipX = false;
                _dashAnim.SetBool("isDash", true);
                _rb.velocity = new Vector2(4, _rb.velocity.y);
            }
            else if (_playerMove.Player.LeftMove.IsPressed())
            {
                _playerSpriteRenderer.flipX = true;
                _rb.velocity = new Vector2(-4, _rb.velocity.y);
            }
        }
        else
        {
            _dashAnim.SetBool("isDash", false);
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }

    IEnumerator IsDamaged()
    {
        _rb.velocity = new Vector2(-1, _rb.velocity.y);
        yield return new WaitForSeconds(1f);
        _dashAnim.SetBool("isDamaged", false);
    }

    void OnBackDash()
    {
        _dashAnim.SetBool("isDamaged", true);

        StartCoroutine(IsDamaged());
    }

    void Update()
    {
        OnDash();

        if (_playerMove.Player.BackDash.IsPressed() && _playerMove.Player.Dash.IsPressed())
        {
            OnBackDash();
        }
    }

}
