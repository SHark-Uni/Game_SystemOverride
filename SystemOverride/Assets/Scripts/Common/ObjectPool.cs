using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Common
{



    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private ObjectPool<T> _pool;
        public ObjectPool<T> pool
        {
            get 
            {
                return _pool;
            }
        }

        public void Init(int capacity, T prefab)
        {
            _stack = new Stack<T>(capacity);
            _capacity = capacity;
            _prefab = prefab;

            T created;
            for (int i = 0; i < capacity; i++)
            {
                created = GameObject.Instantiate<T>(prefab, Vector3.zero, Quaternion.identity);
                created.gameObject.SetActive(false);
                _stack.Push(created);
            }
        }

        public T alloc(Vector3 position, Quaternion rotate)
        {
            T ret = null;

            bool IsEmpty;
            IsEmpty = _stack.TryPop(out ret);
            if (!IsEmpty)
            {
                ret = GameObject.Instantiate(_prefab, position, rotate);
                ret.OnAlloc();
                ++_capacity;
                return ret;
            }

            ret.gameObject.transform.position = position;
            ret.gameObject.transform.rotation = rotate;
            //»ý¼ºÀÚ ´À³¦
            ret.OnAlloc();
            return ret;
        }

        public void release(T obj)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                obj.OnRelease();
                _stack.Push(obj);
            }
        }

        private T _prefab;
        private Stack<T> _stack;
        private int _capacity;
    }
}

