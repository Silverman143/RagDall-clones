using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberTMP;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lifeTime = 1f;
    private bool _isActivae = false;
    private Vector3 _moveVector;


    private void FixedUpdate()
    {
        if (_isActivae)
        {
            Move();
            LifeScale();
        }

    }

    private void SetText(string value)
    {
        _numberTMP.text = value;
    }

    private void GetMoveVector()
    {
        _moveVector = new Vector3(0, 1, 0);

    }

    public void Activate(float value, Vector3 startPos, bool damage)
    {
        if (damage)
        {
            _numberTMP.color = Color.red;
        }
        else
        {
            _numberTMP.color = Color.green;
        }

        int intValue = (int)value;                                          ///Change value to int
        SetText(intValue.ToString());
        transform.position = startPos;
        transform.localScale = Vector3.zero;
        GetMoveVector();
        _isActivae = true;
        StartCoroutine(LifeTime());

    }

    private void Move()
    {
        transform.localPosition += _moveVector * _moveSpeed * Time.deltaTime;
    }

    private void LifeScale()
    {
        if (transform.localScale.magnitude < 3)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
        }

    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        _isActivae = false;
        _moveVector = Vector3.zero;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
