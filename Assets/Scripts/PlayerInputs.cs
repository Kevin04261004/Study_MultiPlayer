using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public Vector2 move;
    public bool jump;
    public bool sprint;
    public bool aiming;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnAiming(InputValue value)
    {
        AimingInput(value.isPressed);
    }

    public void OnShoot(InputValue value)
    {
        
    }
#endif
    public void MoveInput(Vector2 newMoveDir)
    {
        move = newMoveDir;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void AimingInput(bool newAimingState)
    {
        aiming = newAimingState;
    }

    public void ShootInput(bool newShootingState)
    {
        if (newShootingState)
        { // 총 쏘는 거.
            
        }
    }

    public Vector3 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }
}
