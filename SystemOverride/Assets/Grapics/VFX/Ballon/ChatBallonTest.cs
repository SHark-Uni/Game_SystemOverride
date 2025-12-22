using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBallonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(transform.position);
            ChatBallon ballon = ChatBallonManager.instance.SpawnChatBallon("æ»≥Á«œººø‰", transform.position, 0.7f);
        }
    }
}
