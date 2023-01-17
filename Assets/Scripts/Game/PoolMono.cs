using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private Transform _container;

    private List<T> _pool;


    public PoolMono(T prefab, int amount, Transform container, bool autoExpand)
    {
        _prefab = prefab;
        _container = container;
        _autoExpand = autoExpand;
        CreatePool(amount);


    }

    private void CreatePool(int amount)
    {
        _pool = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            CreateObject();
        }
    }

    public int Count()
    {
        return _pool.Count;
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var newObject = GameObject.Instantiate(_prefab, _container);
        newObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(newObject);
        return newObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeSelf)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out T element))
        {
            return element;
        }

        if (_autoExpand)
        {
            element = CreateObject();
            return element;
        }

        throw new System.Exception($" There is no free element in Pool of type {typeof(T)}");
    }
}