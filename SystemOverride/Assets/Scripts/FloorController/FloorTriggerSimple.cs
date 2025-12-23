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
        if (!other.CompareTag("Player")) return;

        if (teleportTarget == null)
        {
            Debug.LogWarning($"[FloorTriggerSimple] teleportTarget이 설정되지 않았습니다: {name}");
            return;
        }

        if (floorController != null)
        {
            // FloorController가 있으면 그쪽 메서드 통해 이동
            floorController.TeleportPlayer(teleportTarget);
        }
        else
        {
            // 간단하게 직접 이동
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
            other.transform.position = teleportTarget.position;
            Debug.Log($"[FloorTriggerSimple] Player teleported to {teleportTarget.name} by {name}");
        }
    }
}