using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BasePhysXBullet : NetworkBehaviour
{
    [SerializeField] protected float _speed;
    [Networked] protected TickTimer _lifeTime { get; set; }
    protected Rigidbody _rb;

    public void Init(Vector3 forward)
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, 5.0f);
        TryGetComponent(out _rb);
        _rb.velocity = forward;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_lifeTime.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }
    }
}
