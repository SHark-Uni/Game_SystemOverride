using System.Collections;
using System.Collections.Generic;
using Scripts.Common;

using UnityEngine;

//VFX prefab에 등록하는 스크립트.

//외부에서 매니저에게 이펙트 출력요청 
//Instantiate하면 이 스크립트가 실행됨. 

//지속시간 만큼, 애니메이션 출력하다가 , 파괴해야함.
//풀에 있다면 -> 풀에 반환 . 아니라면 그냥 파괴.

public class Entity_VFX : MonoBehaviour, IPoolable
{
    private Animator _am;
    private eVFXId _id;

    void Awake()
    {
        _am = GetComponentInChildren<Animator>();
    }

    public void ActiveEffect(float duration)
    {
        StartCoroutine(EffectRoutine(duration));
    }

    public void SetId(eVFXId id)
    {
        _id = id;
    }

    private IEnumerator EffectRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        VFXManager.instance.DestroyEffect(_id, this);
    }

    public void OnAlloc()
    {
        
    }
    public void OnRelease()
    {
        
    }
}
