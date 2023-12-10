using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookRotate : MonoBehaviour
{
    private PlayerInputs _input;
    /* inspector */
    [SerializeField] [Range(0f,0.2f)] private float _rotationSmoothTime = 0.12f;
    private float _rotationVelocity;
    private void Awake()
    {
        TryGetComponent(out _input);
    }

    private void Update()
    {
        if (_input.aiming)
        {
            Aiming();
        }
    }

    private void Aiming()
    {
        Vector3 mousePos = _input.GetMouseScreenPosition();
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
