using Scripts.Player;
using Scripts.Player.Bullets;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FloorTriggerSimple : MonoBehaviour
{
    // Inspector에 이동시킬 Transform을 드래그하세요.
    public Transform Floor1;
    public Transform Floor2_Start;
    public Transform Floor2;
    public Transform Floor3_Start;
    public Transform Floor3;
    // public Transform Boss;
    public Transform Player;

    // 만약 FloorController를 통해 관리하고 싶으면 할당해도 됩니다(선택)
    public FloorController floorController;

    [Tooltip("총알 위치와 플로어 위치 비교 시 허용되는 x축 거리")]
    public float positionTolerance = 1f;

    void Reset()
    {
        // Collider2D가 Trigger로 설정되어 있는지 확인
        var col = GetComponent<Collider2D>();
        Debug.Log(col);
        // Trigger로 설정
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //by junGi
        Bullet bullet = other.GetComponent<Bullet>();
        //Bullet이 아닌경우
        if (bullet == null)
        {
            Debug.Log("Is Not Bullet!");
            return;
        }


        float bulletY = bullet.transform.position.y;
        if (bullet.IsHackingBullet)
        {
            //이동하면 됩니다.
            if (Mathf.Abs(bulletY - Floor1.position.y) <= positionTolerance)
            {
                floorController.TeleportPlayer(Floor2_Start);
            }
            else if (Mathf.Abs(bulletY - Floor2.position.y) <= positionTolerance)
            {
                floorController.TeleportPlayer(Floor3_Start);
            }
            else if (Mathf.Abs(bulletY - Floor3.position.y) <= positionTolerance)
            {
                floorController.TeleportToBoss();
            }
            else
            {
                Debug.Log("빗맞춤");
            }
        }

        // teleportTarget이 설정되어 있는지 확인
        if (Floor1 == null && Floor2 == null && Floor3 == null && Floor2_Start == null && Floor3_Start == null)
        {
            Debug.LogWarning("teleportTarget이 설정되지 않음요");
            return;
        }
        // FloorController가 할당되어 있으면 그쪽을 통해 이동
        /*if (floorController != null)
        {
            // FloorController가 있으면 그쪽 메서드 통해 이동
            floorController.TeleportPlayer(teleportTarget);
        }
        // FloorController가 없으면 직접 이동
        {
            // 플레이어의 Rigidbody2D 컴포넌트 가져오기
            var rb = other.GetComponent<Rigidbody2D>();
            // Rigidbody2D가 있다면 속도를 0으로 초기화해서 순간이동 충돌 문제 방지
            if (rb != null) rb.velocity = Vector2.zero;
            // 플레이어 위치를 teleportTarget 위치로 설정
            other.transform.position = teleportTarget.position;
            Debug.Log($"[FloorTriggerSimple] Bullet teleported to {teleportTarget.name} by {name}");
        }*/
    }
}