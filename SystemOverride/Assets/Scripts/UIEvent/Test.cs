using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static int staticNum;
    public int num;

    private void Start()
    {
        staticNum = staticNum + 1;
        Debug.LogFormat("{0} 오브젝트의, Test 인스턴스의 num 값 {1}, Test클래스의 staticNum의 값 {2}",gameObject.name,num,staticNum);
    }
}
