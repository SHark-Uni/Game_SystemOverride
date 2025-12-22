using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();

        // 에디터에서 Play 중일 때는 Application.Quit()가 동작하지 않으니,
        // 테스트용으로 Play 모드를 꺼줍니다.
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
