using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace C
{
    public class SceneManager3 : MonoBehaviour
    {
        public void OnClickStart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OptionScene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("StorageScene");
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

