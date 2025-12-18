using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    public SoundDataBase _clips;
    private Dictionary<string, AudioClip> _clipCache;
    public Entity_SFX _prefab;
    [SerializeField] private AudioSource _BGM;
    public float volume;

    private ObjectPool<Entity_SFX> _audioPool;
    public static SoundManager instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            Init();
            return;
        }
        Destroy(this);
        return;
    }

    private void Init()
    {
        _audioPool = new ObjectPool<Entity_SFX>();
        _audioPool.Init(24, _prefab);

        _BGM = GetComponent<AudioSource>();
        volume = 1f;
    }

    private void Start()
    {
        _clipCache = _clips.GetDictionary();
    }

    public void PlaySFX(string key, float delay, Vector3 pos)
    {
        AudioClip clip;

        if (!_clipCache.TryGetValue(key, out clip))
        {
            Debug.Log("Wrong Key");
            return;
        }

        Entity_SFX ret = _audioPool.alloc(pos, Quaternion.identity);
        StartCoroutine(DelayAndPlaySFX(ret,clip, delay));
        

        ret.gameObject.SetActive(false);
        _audioPool.release(ret);
    }

    public void PlaySFX(string key, Vector3 pos)
    {
        AudioClip clip;
        if (!_clipCache.TryGetValue(key, out clip))
        {
            Debug.Log("Wrong Key");
            return;
        }
        Entity_SFX ret = _audioPool.alloc(pos, Quaternion.identity);

        ret.gameObject.SetActive(true);
        ret.PlayClip(clip, volume);

        StartCoroutine(DelaySFX(ret, clip.length));
        Debug.Log("실행완료!");
    }

    private IEnumerator DelaySFX(Entity_SFX sfx, float delay)
    { 
        yield return new WaitForSeconds(delay);

        sfx.gameObject.SetActive(false);
        _audioPool.release(sfx);
    }

    private IEnumerator DelayAndPlaySFX(Entity_SFX sfx, AudioClip clip, float Predelay)
    {
        yield return new WaitForSeconds(Predelay);

        sfx.gameObject.SetActive(true);
        sfx.PlayClip(clip, volume);

        yield return new WaitForSeconds(clip.length);
        sfx.gameObject.SetActive(false);
        _audioPool.release(sfx);
    }

    public void PlayBGM(AudioClip clip)
    {
        _BGM.clip = clip;
        _BGM.volume = volume;
        _BGM.loop = true;

        _BGM.Play();
    }

    public void ChangeBGM(AudioClip clip)
    {
        _BGM.Stop();
        _BGM.clip = clip;
        _BGM.Play();
    }

}
