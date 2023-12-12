using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerLookRotate : NetworkBehaviour
{
    [SerializeField]private NetworkInputData _data;
    /* inspector */
    [SerializeField] [Range(0f,0.2f)] private float _rotationSmoothTime;
    private float _rotationVelocity;
    public override void FixedUpdateNetwork()
    {
        
        if (!GetInput(out _data))
        {
            return;
        }

        if (_data.aiming)
        {
            Aiming();
        }
    }

    public void Aiming()
    {
        Vector2 mousePos = _data.mousePos;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseDir = (hit.point - transform.position).normalized;
            float targetRotation = Mathf.Atan2(mouseDir.x, mouseDir.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
}
