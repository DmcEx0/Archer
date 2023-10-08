using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _prefab;
    private bool _isAutoExpand;
    private Transform _container;

    private Queue<T> _pool;

    public ObjectPool(T prefab, int count, Transform container)
    {
        _prefab = prefab;
        _container = container;
        FirstElement = prefab;

        CreatePool(count);
    }

    public T FirstElement {  get; private set; }

    public bool HasFreeElement(out T element)
    {
        foreach (var elementInPool in _pool)
        {
            if (elementInPool.gameObject.activeInHierarchy == false)
            {
                element = elementInPool;
                elementInPool.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

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
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = GameObject.Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isActiveByDefault);

        _pool.Enqueue(createdObject);
        return createdObject;
    }
}