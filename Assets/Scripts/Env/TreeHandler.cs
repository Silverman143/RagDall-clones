using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHandler : ActionTriggerHandler
{
    [SerializeField] private int strength = 3;
    [SerializeField] private Animator _animator;

    private BoxCollider _boxCollider;

    private bool _isActive = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void GetHit()
    {
        if (_isActive)
        {
            strength--;
            if(strength == 0)
            {
                Deactivate();
                _boxCollider.enabled = false;
                _isActive = false;
                transform.rotation = Quaternion.LookRotation(transform.position - GameHandler._playerController.transform.position);
                _animator.SetTrigger("Falls");

            }
            else
            {
                _animator.SetTrigger("GetHit");
            }
        }
    }

    public override void OnAction()
    {
        GetHit();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<AxeController>(out AxeController axe))
    //    {
    //        axe.DeactivateHits();
    //        GetHit();
    //    }
    //}

    //IEnumerator DestroingCorutine()
    //{

    //}
}
