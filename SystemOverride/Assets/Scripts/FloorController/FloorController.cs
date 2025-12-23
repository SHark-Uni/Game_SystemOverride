using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorController : MonoBehaviour
{
    public Transform player;
    public Transform bossPoint; // 보스 지점(같은 씬)

    void Start()
    {
        if (player == null) Debug.LogWarning("FloorController: player가 할당되지 않았습니다.");
    }

    // 트리거나 다른 스크립트에서 호출해서 플레이어를 순간이동시킬 때 사용
    public void TeleportPlayer(Transform target)
    {
        if (player == null || target == null) return;

        // Rigidbody2D가 있다면 속도를 0으로 초기화해서 순간이동 충돌 문제 방지
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;

        player.position = target.position;
        Debug.Log($"[FloorController] Teleported player to {target.name} at {target.position}");
    }

    // 보스 지점으로 이동 (옵션)
    public void TeleportToBoss()
    {
        if (bossPoint == null)
        {
            Debug.LogWarning("[FloorController] bossPoint가 설정되지 않았습니다.");
            return;
        }
        TeleportPlayer(bossPoint);
    }
}
