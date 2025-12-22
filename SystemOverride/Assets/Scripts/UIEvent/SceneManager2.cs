using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace B
{
    public class SceneManager2 : MonoBehaviour
    {
        public void OnClickStart1()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OptionScene");;
        }

        public void OnClickStart2()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("StorageScene"); ;
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
