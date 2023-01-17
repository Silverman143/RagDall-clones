using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersEffect : MonoBehaviour
{
    [SerializeField] private NumberHandler _pref;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private int _startAmount;
    [SerializeField] private Transform _container;

    private PoolMono<NumberHandler> _numbersTextPool;

    private void Awake()
    {
        _numbersTextPool = new PoolMono<NumberHandler>(_pref, _startAmount, _container, _autoExpand);
    }


    public void Activate(float value, Vector3 startPos, bool damage)
    {

        NumberHandler handler = _numbersTextPool.GetFreeElement();
        startPos.y += 2;
        handler.gameObject.SetActive(true);
        handler.Activate(value, startPos, damage);
    }
}
