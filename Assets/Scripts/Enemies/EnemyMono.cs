using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EnemyMono : MonoBehaviour
{
    [SerializeField] private GameObject _targetIndicator;
    [SerializeField] private Transform _model;
    [SerializeField] public Status _status;
    public List<HumanMono> _targets;

    public Animator _animator;
    public NavMeshAgent _navMeshAgent;
    public HealthSystem _healthSystem;

    public UnityEvent<EnemyMono> OnDead;

    public virtual void GetDamage(int value)
    {

    }

    public virtual void SetTargetIndicator(bool value)
    {
        _targetIndicator.SetActive(value);
    }

    public virtual void Rotate()
    {
        switch (_status)
        {
            case Status.Idle:
                break;
            case Status.Walking:
                _model.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity);
                break;
            case Status.Attack:
                _model.transform.rotation = Quaternion.LookRotation(_targets[0].transform.position - transform.position);
                break;
        }
    }
}
