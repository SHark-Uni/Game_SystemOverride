using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossMove2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private float inputX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // A/D, ←/→ 입력 (기본 Horizontal 축)
        inputX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
    }

    private void FixedUpdate()
    {
        // 기존 Y 속도는 유지하면서 X만 변경
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
    }
}