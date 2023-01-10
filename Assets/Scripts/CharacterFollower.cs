using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollower : MonoBehaviour
{
    [SerializeField] private Transform _characterTransform;
    [Range(0, 1)]
    [SerializeField] private float _smoothnessSpeed;
    [SerializeField] private bool _distant;
    [SerializeField] private float _distantVolume;
    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _characterTransform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetPos = _characterTransform.position + _offset;
        if (_distant)
        {
            targetPos += _offset * _distantVolume;
        }
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, _smoothnessSpeed);
        transform.position = smoothPos;
    }

    public void SetDistance(bool isDistant)
    {
        if (isDistant)
        {
            _distant = true;
            //FogHandler.SetFog(43, 51);
        }
        else
        {
            _distant = false;
            //FogHandler.Reset();
        }
    }
}
