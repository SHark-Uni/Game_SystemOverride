using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace D
{
    public class SceneManager4 : MonoBehaviour
    {
        //public void MoveScene()
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("sceneName");
        //}

        public void OnClickStart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        // 씬 이름으로 이동
        public void LoadSceneByName(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        // 씬 인덱스로 이동
        public void LoadSceneByIndex(int index)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        // 게임 종료 (PC 빌드 전용)
        public void QuitGame()
        {
            Application.Quit();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}