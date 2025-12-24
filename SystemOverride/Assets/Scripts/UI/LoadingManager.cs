using Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace Scripts.UI
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager instance { get; private set; }

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

        public GameObject _loadPannelPrefab;
        private AsyncOperation _asyncOp;

        public void ChangeSceneWithLoadingPanel(eSceneType scene, Vector3 pos, Action OnEnterScene)
        {
            StartCoroutine(LoadGameRoutine(scene, pos, OnEnterScene));
        }

        private IEnumerator LoadGameRoutine(eSceneType scene, Vector3 pos, Action OnEnterScene)
        {
            GameObject canvas = Instantiate(_loadPannelPrefab, pos, Quaternion.identity);
            Image scrollbar = canvas.GetComponentInChildren<Image>();
            canvas.SetActive(true);

            //메인게임씬 로딩
            _asyncOp = SceneLoader.instance.LoadAsyncScene(scene);
            _asyncOp.allowSceneActivation = false;

            float timer = 0f;
            while (!_asyncOp.isDone)
            {
                if (_asyncOp.progress < 0.9f)
                {
                    scrollbar.fillAmount = _asyncOp.progress;
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    scrollbar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                    if (scrollbar.fillAmount >= 1f)
                    {
                        _asyncOp.allowSceneActivation = true;
                        Destroy(canvas);
                        //SoundManager.instance.ChangeBGM(scene.ToString());
                        OnEnterScene.Invoke();
                        yield break;
                    }
                }
                yield return null;
            }
        }

    }
}

