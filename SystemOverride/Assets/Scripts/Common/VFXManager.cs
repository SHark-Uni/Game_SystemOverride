using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Common
{
    public class VFXManager : MonoBehaviour
    {
        //Pooling이 필요한 VFX를 관리하기 위한 매니저
        //뭔가 많이 쓰이지 않는거면 여기서 관리x. 그냥 로직과 결합하는게 나음.(모든걸 Pooling할순없음)
       
        public static VFXManager _instance;

        [Header("onHitVFX Details")]
        [SerializeField] private Entity_VFX _hitVfxPrefab;
        [SerializeField] private float _hitVfxDuration;
        [SerializeField] private int _hitVfxPoolCapacity;

        [Header("Laser VFX")]
        [SerializeField] private Entity_VFX _LaserPrefab;

        private ObjectPool<Entity_VFX> _HitVFXPool;
        private ObjectPool<Entity_VFX> _LaserPool;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                
                _hitVfxDuration = 0.25f;
                _hitVfxPoolCapacity = 256;

                Init();
                DontDestroyOnLoad(this);
            }
            else 
            {
                Destroy(this);
            }
        }

        public static VFXManager instance
        {
            get
            {
                return _instance;
            }
        }

        public void PlayEffect(eVFXId id, Vector3 position, Quaternion rotate)
        {
            switch (id)
            {
                case eVFXId.onHitVFX:
                    PlayHitVFX(position, rotate);
                    break;
                case eVFXId.laserVFX:
                    PlayLaserVFX(position, rotate);
                    break;
                default:
                    break;
            }
        }

        public void DestroyEffect(eVFXId id, Entity_VFX vfx)
        {
            switch (id)
            {
                case eVFXId.onHitVFX:
                    _HitVFXPool.release(vfx);
                    break;
                case eVFXId.laserVFX:
                    _LaserPool.release(vfx);
                    break;
                default:
                    break;
            }
        }

        private void PlayLaserVFX(Vector3 position, Quaternion rotate)
        {
            Entity_VFX laser = _LaserPool.alloc(position, rotate);

            laser.SetId(eVFXId.laserVFX);
            laser.gameObject.SetActive(true);

            laser.ActiveEffect(0.35f);
        }

        private void PlayHitVFX(Vector3 position, Quaternion roate)
        {
            Entity_VFX ret = _HitVFXPool.alloc(position, roate);
            ret.SetId(eVFXId.onHitVFX);
            ret.gameObject.SetActive(true);

            ret.ActiveEffect(_hitVfxDuration);
        }
        private void Init()
        {
            _HitVFXPool = new ObjectPool<Entity_VFX>();
            _LaserPool = new ObjectPool<Entity_VFX>();

            _HitVFXPool.Init(_hitVfxPoolCapacity, _hitVfxPrefab);
            _LaserPool.Init(48, _LaserPrefab);
        }
    }
}

