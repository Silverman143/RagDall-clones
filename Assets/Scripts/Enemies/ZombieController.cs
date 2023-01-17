using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : EnemyMono
{
    [Header("Parameters")]
    [SerializeField] private int _attackForce = 1;
    [SerializeField] private float _attackSpeed = 5f;

    // Components.
    [SerializeField] private Status _status;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private HealthSystem _healthSystem;

    private List<HumanMono> _targets;
    private float _attackTimer = 0;
    [SerializeField]private bool _isAttacking = false;

    private void Awake()
    {
        _status = Status.Idle;
        _targets = new List<HumanMono>();
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
    }

    private void Update()
    {
        switch (_status)
        {
            case Status.Idle:
                break;
            case Status.Walking:
                Move();
                break;
            case Status.Attack:
                Attack();
                break;
        }

        StatusSwitcher();
        Animate();
    }

    private void StatusSwitcher()
    {
        if (_targets.Count > 0 && Vector3.Distance(transform.position, _targets[0].transform.position) > _navMeshAgent.stoppingDistance)
        {
            _status = Status.Walking;
        }
        else if (_targets.Count > 0 && Vector3.Distance(transform.position, _targets[0].transform.position) <= _navMeshAgent.stoppingDistance)
        {
            _status = Status.Attack;
        }
        else
        {
            _status = Status.Idle;
        }
    }

    private void Move()
    {
        if (_targets.Count > 0)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_targets[0].transform.position);
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }
    }

    private void Animate()
    {
        if (_status == Status.Walking)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    private void Attack()
    {
        if (_attackTimer <= 0 && !_isAttacking)
        {
            Debug.Log("Attack");
            StartCoroutine(AttackCoroutine());
            _attackTimer = _attackSpeed;
        }
        else
        {
            _attackTimer -= Time.deltaTime;
        }
    }

    private void GetDamage(int value)
    {
        _healthSystem.GetDamage(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            _targets.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            _targets.Remove(player);
        }
    }

    IEnumerator AttackCoroutine()
    {
        _isAttacking = true;

        _animator.SetTrigger("Hit");

        while (!_animator.GetCurrentAnimatorStateInfo(2).IsName("Hit"))
        {
            yield return null;
        }

        while (_animator.GetCurrentAnimatorStateInfo(2).IsName("Hit") && _animator.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.5f)
        {
            yield return null;
        }

        _targets[0].GetDamage(_attackForce);

        while (_animator.GetCurrentAnimatorStateInfo(2).IsName("Hit"))
        {
            yield return null;
        }
        _isAttacking = false;
    }
}
