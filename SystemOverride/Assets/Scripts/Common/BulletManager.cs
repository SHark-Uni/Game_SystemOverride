using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common;
using Scripts.Player.Bullets;

namespace Scripts.Common
{
    public class BulletManager : MonoBehaviour
    {
        public Bullet _bulletPrefab;
        public static BulletManager _instance;
        public static BulletManager instance
        {
            get { return _instance; }
        }
        private ObjectPool<Bullet> _bulletPool;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                _bulletPool = new ObjectPool<Bullet>();
                _bulletPool.Init(128, _bulletPrefab);
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(this);
            return;
        }

        public Bullet CreatedBullet(Vector3 position, Quaternion rotate)
        {
            return _bulletPool.alloc(position, rotate);
        }

        public void DestroyBullet(Bullet bullet)
        {
            _bulletPool.release(bullet);
        }
    }
}

