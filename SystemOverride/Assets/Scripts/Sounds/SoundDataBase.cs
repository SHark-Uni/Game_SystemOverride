using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SfxDatabase", menuName = "Audio/SfxDatabase")]
public class SoundDataBase : ScriptableObject
{
    [System.Serializable]
    public struct AudioData
    {
        public string _name;
        public AudioClip _clip;
    }
    //필수적인 것들 미리 Load
    public List<AudioData> _audioClips;

    public Dictionary<string, AudioClip> GetDictionary()
    {
        Dictionary<string, AudioClip> ret = new Dictionary<string, AudioClip>();

        for (int i = 0; i < _audioClips.Count; i++)
        {
            if (_audioClips[i]._name == null)
                continue;
            ret[_audioClips[i]._name] = _audioClips[i]._clip;
        }

        return ret;
    }

}
