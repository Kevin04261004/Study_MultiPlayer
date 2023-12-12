using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BaseBullet : NetworkBehaviour
{
    [SerializeField] protected float _speed;
    [Networked] protected TickTimer _lifeTime { get; set; }

    public void Init()
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_lifeTime.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }
        transform.position += transform.forward * Runner.DeltaTime * _speed;
    }
    
}
