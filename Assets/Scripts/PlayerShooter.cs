using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;

enum EBulletType
{
    BaseBullet,
    
}

public class PlayerShooter : NetworkBehaviour
{
    private EBulletType _bulletType;
    [SerializeField] private BasePhysXBullet[] _bullet;
    [Networked] private TickTimer _delay { get; set; }
    private Vector3 _forward;
    private NetworkInputData _data;
    private PlayerLookRotate _playerLookRotate;
    
    private void Awake()
    {
        _playerLookRotate = GetComponent<PlayerLookRotate>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _data))
        {
            return;
        }

        if (_delay.ExpiredOrNotRunning(Runner))
        {
            _forward = gameObject.transform.forward;
            if (_data.shoot)
            {
                Shot();
            }
        }
    }

    private void Shot()
    {
        _playerLookRotate.Aiming();
        switch (_bulletType)
        {
            case EBulletType.BaseBullet:
                _delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                var transform1 = transform;
                Runner.Spawn(_bullet[(int)_bulletType], transform1.position,transform1.rotation,Object.InputAuthority,
                    (runner, o) =>
                    {
                        o.GetComponent<BasePhysXBullet>().Init(_forward * 10);
                    });
                break;
            default:
                Debug.Assert(true, "Add case");
                break;
        }        
    }
}
