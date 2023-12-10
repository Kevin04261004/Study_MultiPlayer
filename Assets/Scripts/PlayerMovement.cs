using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _speed;
    private float _curSpeed;
    private bool _atGround = false;
    private Vector3 _groundCheckPos;
    private readonly float _groundCheckRadius = 0.28f;
    private const float SPEED_OFFSET = 0.1f;
    private float _currentHorizontalSpeed;
    private Vector3 _velocity;
    private float _targetRotation;
    private float _rotationVelocity;
    private Vector3 _targetDirection;
    private float _verticalVelocity;
    private float _terminalVelocity = 50f;
    private const float GRAVITY = -15f;

    /* Classes Or Property */
    #if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
    #endif
    private PlayerInputs _input;
    private CharacterController _controller;
    
    /* inspector */
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _speedChangeLate = 10f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] [Range(0f,0.2f)] private float _rotationSmoothTime = 0.12f;

    private void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _controller);
#if ENABLE_INPUT_SYSTEM
        TryGetComponent(out _playerInput);
#else
        Debug.Assert(true, "No InputSystem");
#endif
                
    }

    private void Update()
    {
        Move();
        
        GroundCheck();
        Gravity();
    }
    
    private void Move()
    {
        _curSpeed = _input.sprint ? _sprintSpeed : _moveSpeed;
        if (_input.move == Vector2.zero)
        {
            _curSpeed = 0.0f;
        }

        _velocity = _controller.velocity;
        _currentHorizontalSpeed = new Vector3(_velocity.x, 0.0f, _velocity.z).magnitude;

        if (_currentHorizontalSpeed < _curSpeed - SPEED_OFFSET ||
            _currentHorizontalSpeed > _curSpeed + SPEED_OFFSET)
        {
            _speed = Mathf.Lerp(_currentHorizontalSpeed, _curSpeed * 1f, Time.deltaTime * _speedChangeLate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = _curSpeed;
        }
        
        // 애니메이션 추가.
        
        //
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);

            if (!_input.aiming)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime)
            + new Vector3(0,_verticalVelocity, 0) * Time.deltaTime);
        // 나중에 중력 추가하기.
        
        // 애니메이션 추가
        
        //
    }

    private void GroundCheck()
    {
        _groundCheckPos = transform.position;
        _atGround = Physics.CheckSphere(_groundCheckPos, _groundCheckRadius, _groundLayerMask,
            QueryTriggerInteraction.Ignore);
    }

    private void Gravity()
    {
        if (_atGround)
        {
            _verticalVelocity = 0f;
        }
        else
        {
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += GRAVITY * Time.deltaTime;
            }   
        }
    }
}
