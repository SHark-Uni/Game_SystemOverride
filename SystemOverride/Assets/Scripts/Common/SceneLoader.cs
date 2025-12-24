using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Common
{
    public enum eSceneType
    {
        _Title,
        _Main,
        _GameScene,
        _Boss,
        _Ending,
    }

    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                return;
            }
            Destroy(this);
            return;
        }


        public void LoadScene(eSceneType type)
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(type.ToString());
            SoundManager.instance.ChangeBGM(type.ToString());
        }

        public void ReloadCurrentScene(eSceneType type)
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public AsyncOperation LoadAsyncScene(eSceneType type)
        {
            Time.timeScale = 1f;

            return SceneManager.LoadSceneAsync(type.ToString());
        }

    }
}

