using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FloorTriggerSimple : MonoBehaviour
{
    // Inspector에 이동시킬 Transform을 드래그하세요.
    public Transform teleportTarget;

    // 만약 FloorController를 통해 관리하고 싶으면 할당해도 됩니다(선택)
    public FloorController floorController;

    void Reset()
    {
        // Collider2D가 Trigger로 설정되어 있는지 확인
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bullet 태그인지 확인
        if (!other.CompareTag("Bullet")) return;

        // teleportTarget이 설정되어 있는지 확인
        if (teleportTarget == null)
        {
            Debug.LogWarning($"[FloorTriggerSimple] teleportTarget이 설정되지 않았습니다: {name}");
            return;
        }
        // FloorController가 할당되어 있으면 그쪽을 통해 이동
        if (floorController != null)
        {
            // FloorController가 있으면 그쪽 메서드 통해 이동
            floorController.TeleportPlayer(teleportTarget);
        }
        else
        // FloorController가 없으면 직접 이동
        {
            // 플레이어의 Rigidbody2D 컴포넌트 가져오기
            var rb = other.GetComponent<Rigidbody2D>();
            // Rigidbody2D가 있다면 속도를 0으로 초기화해서 순간이동 충돌 문제 방지
            if (rb != null) rb.velocity = Vector2.zero;
            // 플레이어 위치를 teleportTarget 위치로 설정
            other.transform.position = teleportTarget.position;
            Debug.Log($"[FloorTriggerSimple] Bullet teleported to {teleportTarget.name} by {name}");
        }
    }
}