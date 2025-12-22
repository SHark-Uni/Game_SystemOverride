using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBallonManager : MonoBehaviour
{
    public static ChatBallonManager instance {  get; private set; }
    public GameObject _ChatBallonPrefab;

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

    //쓰는 측에서 Destroy.
    public ChatBallon SpawnChatBallon(string text, Vector3 pos, float fontSize)
    {
        Vector3 upperChatPos = pos + new Vector3(0, 1.5f, 0);
        //Vector3 renderPos = Camera.main.WorldToScreenPoint(upperChatPos);

        ChatBallon ret = Instantiate(_ChatBallonPrefab, upperChatPos, Quaternion.identity).GetComponent<ChatBallon>();
        ret.SetText(text, fontSize);
        return ret;
    }
}
