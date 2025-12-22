using UnityEngine;
using Scipts.Boss;

// 맵 패턴 테스트용 컨트롤러
namespace Scripts.Boss
{
    public class MapController : MonoBehaviour
    {

        public MapHazardSpawner _spawner;

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("[Test] 낙하물 생성 시작!");
                _spawner.StartSpawning();
            }


            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("[Test] 낙하물 생성 중지.");
                _spawner.StopSpawning();
            }
        }
    }
}