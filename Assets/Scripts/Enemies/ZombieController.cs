using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : EnemyMono
{
    [Header("Parameters")]
    [SerializeField] private int _attackForce = 1;
    [SerializeField] private float _attackSpeed = 5f;

    private float _attackTimer = 0;
    [SerializeField]private bool _isAttacking = false;

    // VFX.
    [SerializeField] private ParticleSystem _deadVFX;

    private void Awake()
    {
        _isActive = true;
        _status = Status.Idle;
        _targets = new List<HumanMono>();
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
        _healthSystem.OnHealthFinished.AddListener(Dying);
    }


    private void Update()
    {
        if (_isActive)
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
            Rotate();
        }
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
            StartCoroutine(AttackCoroutine());
            _attackTimer = _attackSpeed;
        }
        else
        {
            _attackTimer -= Time.deltaTime;
        }
    }

    public override void GetDamage(int value)
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

    private void Dying()
    {
        _isActive = false;
        OnDead.Invoke(this);
        _animator.SetTrigger("Dead");
        _deadVFX.Play();
        SetTargetIndicator(false);
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
