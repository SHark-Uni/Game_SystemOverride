using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 100f;
    public bool timerIsRunning = true;

    // 불러올 게임오버 씬 이름
    public string gameOverSceneName = "OverScene";

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0f;
            timerIsRunning = false;

            Debug.Log("Time Over!");
            LoadGameOver();
        }
    }

    void LoadGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}
