using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly bool _isAutoExpand = true;
        private readonly Transform _container;

        private Queue<T> _pool;

        public ObjectPool(T prefab, int count, Transform container)
        {
            _prefab = prefab;
            _container = container;
            FirstElement = prefab;

            CreatePool(count);
        }

        public T FirstElement { get; private set; }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;

            if (_isAutoExpand)
                return CreateObject(true);

            throw new Exception($"There is no free element in pool of type {typeof(T)}");
        }

        private void CreatePool(int count)
        {
            _pool = new Queue<T>();

            for (int i = 0; i < count; i++)
            {
                _ = CreateObject();
            }
        }

        private bool HasFreeElement(out T element)
        {
            foreach (var elementInPool in _pool)
            {
                if (elementInPool.isActiveAndEnabled == false)
                {
                    element = elementInPool;
                    elementInPool.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = GameObject.Instantiate(_prefab, _container);
            createdObject.gameObject.SetActive(isActiveByDefault);

            _pool.Enqueue(createdObject);
            return createdObject;
        }
    }
}