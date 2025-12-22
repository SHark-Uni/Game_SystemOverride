using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace A
{
    public class SceneManager1 : MonoBehaviour
    {
        public void OnClickStart0()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void LoadGame()
        {
            if (SaveManager.HasSave())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.Log("There is no Saved Game!");
            }
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

        public void OnClickStart1()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OptionScene");
        }

        // 게임 종료 
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

