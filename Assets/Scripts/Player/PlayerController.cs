using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ActionType
{
    Tree, Ore,
}

public enum Status
{
    Walking, Attack, Hitting, Action, Idle
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : HumanMono
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerData _playerData;

    private Status _characterStatus;
    private PlayerAttackTriggerHandler _attackTriggerHandler;
    private Vector3 _moveVector;
    private float _gravityForce;
    private CharacterController _characterController;
    private HealthSystem _healthSystem;
    private WeaponHandler _weaponHandler;

    private List<ActionTriggerHandler> _actionTriggers;


    private bool _actionIsActive = false;
    private EnemyMono _targetEnemy;
    // Events.

    public delegate void StartAttack();
    public static event StartAttack OnStartAttack;

    [Header("Stats")]
    [SerializeField] private float _attackSpeed = 5f;
    private float _attackTimer = 0;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _healthSystem = GetComponent<HealthSystem>();
        _characterStatus = Status.Walking;
        _actionTriggers = new List<ActionTriggerHandler>();
        _attackTriggerHandler = GetComponentInChildren<PlayerAttackTriggerHandler>();
        _attackTriggerHandler.OnEnemyChanged.AddListener(SetTarget);
        _healthSystem.OnHealthFinished.AddListener(Dead);
        _weaponHandler = GetComponent<WeaponHandler>();
    }

    private void FixedUpdate()
    {
        switch (_characterStatus)
        {
            case Status.Walking:
                Move();
                Gravity();
                break;

            case Status.Action:
                ActionStarter();
                break;

            case Status.Attack:
                //_targetEnemy = _attackTriggerHandler.TargetEnemy;
                Attack();
                break;
        }
        StatusSwitcher();
        Rotate();
        Animate();

    }

    private void Move()
    {
        _moveVector = Vector3.zero;
        _moveVector.x = Joystick.Instance.Horizontal;
        _moveVector.z = Joystick.Instance.Vertical;
        _moveVector.y = _gravityForce;
        _characterController.Move(_moveVector * _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (HasInput())
        {
            Vector3 targetRotation = new Vector3(_moveVector.x, 0, _moveVector.z);
            _model.rotation = Quaternion.LookRotation(targetRotation);
        }
        else if (_characterStatus == Status.Attack)
        {
            Vector3 targetRotation = _targetEnemy.transform.position - transform.position;
            targetRotation.y = 0;
            _model.rotation = Quaternion.LookRotation(targetRotation);
        }
    }

    private void Animate()
    {
        _animator.SetBool("isRunning", HasInput() ? true : false);
    }

    private void Gravity()
    {
        if (!_characterController.isGrounded)
        {
            _gravityForce -= 20 * Time.deltaTime;
        }
        else
        {
            _gravityForce = -1;
        }
    }

    private bool HasInput()
    {
        if (Joystick.Instance.Horizontal != 0 | Joystick.Instance.Vertical != 0) return true;
        else return false;
    }

    private void SetTarget(EnemyMono enemy)
    {
        _targetEnemy = enemy;
    }

    private void StatusSwitcher()
    {
        Status oldStatus = _characterStatus;


        // Define a new character status.

        if (HasInput())
        {
            _characterStatus = Status.Walking;
        }
        else if(_actionTriggers.Count!= 0)
        {

            _characterStatus = Status.Action;
        }
        else if (_targetEnemy != null)
        {
            _characterStatus = Status.Attack;
        }
        else
        {
            _characterStatus = Status.Idle;
        }

        // Changing character status.

        if (oldStatus != _characterStatus)
        {
            string idleAnim = _weaponHandler.GetIdleAnimationName();

            if (_characterStatus == Status.Action)
            {
                _weaponHandler.ActivateActionWeapon(_actionTriggers[0].Type);
            }
            else if (_characterStatus == Status.Attack)
            {
                _weaponHandler.DeactivateActionWeapons();

                if (idleAnim != "Non") _animator.SetBool(idleAnim, true);
                _attackTriggerHandler.UpdateTarget();
            }

            if (_characterStatus != Status.Attack)
            {
                if (idleAnim != "Non") _animator.SetBool(idleAnim, false);
            }

            StopAllCoroutines();
            _actionIsActive = false;
        }

        
    }

    private void RemoveActionTrigger(ActionTriggerHandler trigger)
    {
        trigger.OnFinished -= RemoveActionTrigger;
        _actionTriggers.Remove(trigger);
        StopCoroutine(ActionCoroutine());
    }

    private void ActionStarter()
    {
        if (_attackTimer <= 0 && !_actionIsActive && _actionTriggers.Count != 0)
        {
            StartCoroutine(ActionCoroutine());
            _attackTimer = _attackSpeed;
        }
        else
        {
            _attackTimer -= Time.deltaTime;
        }


    }

    private void Attack()
    {
        if (_targetEnemy != null)
        {
            float attackDistance = _weaponHandler.GetWeaponAttackDistance();
            float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);

            if (_attackTimer <= 0 && !_actionIsActive && distance <= attackDistance)
            {

                StartCoroutine(AttackCoroutine());
            }
            else
            {
                _attackTimer -= Time.deltaTime;
            }
        } 
    }

    public override void GetDamage(int damageForce)
    {
        _healthSystem.GetDamage(damageForce);
    }

    private void Dead()
    {
        // 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ActionTriggerHandler>(out ActionTriggerHandler actionTrigger))
        {
            _actionTriggers.Add(actionTrigger);
            actionTrigger.OnFinished += RemoveActionTrigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ActionTriggerHandler>(out ActionTriggerHandler actionTrigger))
        {
            _actionTriggers.Remove(actionTrigger);
            actionTrigger.OnFinished -= RemoveActionTrigger;
        }
    }

    IEnumerator ActionCoroutine()
    {
        _actionIsActive = true;

        _animator.SetTrigger("HitTree");

        while (!_animator.GetCurrentAnimatorStateInfo(1).IsName("HitTree"))
        {
            yield return null;
        }

        while (_animator.GetCurrentAnimatorStateInfo(1).IsName("HitTree") && _animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.3f)
        {
            yield return null;
        }
        _actionTriggers[0].OnAction();
        VibrationHandler.Hit();

        while (_animator.GetCurrentAnimatorStateInfo(1).IsName("HitTree"))
        {
            yield return null;
        }
        _actionIsActive = false;
    }

    IEnumerator AttackCoroutine()
    {
        _actionIsActive = true;
        WeaponMono weapon = _weaponHandler.GetCurretnWeapon();

        string anim = "Attack";
        _animator.SetTrigger(anim);

        while (!_animator.GetCurrentAnimatorStateInfo(1).IsName(anim))
        {
            yield return null;
        }

        while (_animator.GetCurrentAnimatorStateInfo(1).IsName(anim) && _animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.3f)
        {
            yield return null;
        }
        if(weapon.Type == WeaponType.Melee)
        {
            _targetEnemy.GetDamage(weapon.DamageForce);
            VibrationHandler.Hit();
        }
        while (_animator.GetCurrentAnimatorStateInfo(1).IsName(anim))
        {
            yield return null;
        }

        _actionIsActive = false;
    }
}
