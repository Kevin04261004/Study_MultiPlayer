using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
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
    private readonly float _terminalVelocity = 50f;
    private const float GRAVITY = -15f;
    private NetworkInputData _data;
    /* Classes Or Property */
    private NetworkCharacterControllerPrototype _cc;
    /* inspector */
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _speedChangeLate;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] [Range(0f,0.2f)] private float _rotationSmoothTime = 0.12f;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    // private void Update()
    // {
    //     Move();
    //     
    //     GroundCheck();
    //     Gravity();
    // }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _data))
        {
            return;
        }
        Move();
        
        GroundCheck();
        Gravity();
    }

    private void Move()
    {
        _curSpeed = _data.sprint ? _sprintSpeed : _moveSpeed;
        if (_data.move == Vector3.zero)
        {
            _curSpeed = 0.0f;
        }

        _velocity = _cc.Velocity;
        _currentHorizontalSpeed = new Vector3(_velocity.x, 0.0f, _velocity.z).magnitude;
        print(_currentHorizontalSpeed);
        

        if (_currentHorizontalSpeed < _curSpeed - SPEED_OFFSET ||
            _currentHorizontalSpeed > _curSpeed + SPEED_OFFSET)
        {
            _speed = Mathf.Lerp(_currentHorizontalSpeed, _curSpeed, Runner.DeltaTime * _speedChangeLate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = _curSpeed;
        }
        // 애니메이션 추가.
        
        //
        Vector3 inputDirection = new Vector3(_data.move.x, 0.0f, _data.move.y).normalized;
        
        if (_data.move != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);

            if (!_data.aiming)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _cc.Move(_targetDirection.normalized * (_speed *Runner.DeltaTime)
            + new Vector3(0,_verticalVelocity, 0) *Runner.DeltaTime);
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
                _verticalVelocity += GRAVITY * Runner.DeltaTime;
            }   
        }
    }
}
