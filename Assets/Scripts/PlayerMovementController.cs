using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _animator;

    private Vector3 _moveVector;
    private float _gravityForce;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
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

}
