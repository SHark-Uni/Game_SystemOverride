using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    public SoundDataBase _clips;
    public SoundDataBase _bgms;

    private Dictionary<string, AudioClip> _clipCache;
    private Dictionary<string, AudioClip> _bgmCache;

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
        _audioPool.Init(ConfigManager.SoundSourcePoolSize, _prefab);

        _BGM = GetComponent<AudioSource>();

        volume = 1f;
    }

    private void Start()
    {
        _clipCache = _clips.GetDictionary();
        _bgmCache = _bgms.GetDictionary();

        _BGM.loop = true;
        PlayBGM("_Main");
    }

    //처음 딜레이 발생한다음에, 플레이 몇초뒤 효과음 플레이
    public void PlaySFX(string key, float delay, Vector3 pos)
    {
        AudioClip clip;

        if (!_clipCache.TryGetValue(key, out clip))
        {
            return;
        }

        Entity_SFX ret = _audioPool.alloc(pos, Quaternion.identity);
        StartCoroutine(DelayAndPlaySFX(ret,clip, delay));
    }
    //효과음의 길이만큼 그냥 플레이
    public void PlaySFX(string key, Vector3 pos)
    {
        AudioClip clip;
        if (!_clipCache.TryGetValue(key, out clip))
        {
            return;
        }
        Entity_SFX ret = _audioPool.alloc(pos, Quaternion.identity);

        ret.gameObject.SetActive(true);
        ret.PlayClip(clip, volume);

        StartCoroutine(DelaySFX(ret, clip.length));
    }

    private IEnumerator DelaySFX(Entity_SFX sfx, float delay)
    { 
        yield return new WaitForSeconds(delay);
        //씬전환시, sfx자체가 null이 될 수 있음.
        if (sfx == null)
        {
            yield break;
        }

        //효과음이 끝나고, sfx이 어셈블리를 실행할때! 씬전환이 완료되서 빢 ! 터지는경우
        sfx.gameObject.SetActive(false);
        _audioPool.release(sfx);
    }

    private IEnumerator DelayAndPlaySFX(Entity_SFX sfx, AudioClip clip, float Predelay)
    {
        yield return new WaitForSeconds(Predelay);
        if (sfx == null)
        {
            yield break;
        }
        sfx.gameObject.SetActive(true);
        sfx.PlayClip(clip, volume);

        yield return new WaitForSeconds(clip.length);
        sfx.gameObject.SetActive(false);
        _audioPool.release(sfx);
    }


    private void PlayBGM(string key)
    {
        AudioClip clip;
        if (!_bgmCache.TryGetValue(key, out clip))
        {
            return;
        }
      
        _BGM.clip = clip;
        _BGM.Play();
    }

    public void ChangeBGM(string key)
    {
        AudioClip clip;
        if (!_bgmCache.TryGetValue(key, out clip))
        {
            return;
        }

        _BGM.Stop();
        _BGM.clip = clip;
        _BGM.Play();
    }

    public void SetBGMVolume(float vol)
    {
        _BGM.volume = vol;
    }

    public void SetSFXVolume(float vol)
    {
        volume = vol; 
    }

}
