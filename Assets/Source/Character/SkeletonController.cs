using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;
using InputType = CharacterStateRunner.CharacterStateInputType;


public class SkeletonController : BaseCharacter
{
    enum SkeletonControllerState
    {
        Idle,
        Walking,
        ChasingPlayer,
        Attacking,
    }

    [SerializeField]
    private FloatReference _idleDuration;
    [SerializeField]
    private FloatReference _walkDuration;

    [SerializeField]
    private FloatReference _AttackDistance;

    [SerializeField]
    private FloatReference _senseDistance;

    [SerializeField]
    private FloatReference _senseHeight;

    [SerializeField]
    private FloatReference _attackPrepTime;

    [SerializeField]
    private FloatReference _attackTime;


    [SerializeField]
    private ScriptableCharacterList _playerList;

    [SerializeField]
    private GameObject _pointParticlePrefab;


    private int _walkDirection = 1;

    private SkeletonControllerState _currentState = SkeletonControllerState.Idle;

    private float _currentStateTime;


    void Update()
    {
        var parameters = new Dictionary<InputType, object>
        {
            { InputType.HalfExtents, _collider2D.size / 2 },
            { InputType.ColliderOffset, _collider2D.offset},
        };

        float horizontalAxis = 0;
        var playerInSight = IsPlayerInSight();
        if (_currentState != SkeletonControllerState.Attacking && playerInSight)
        {
            SetState(SkeletonControllerState.ChasingPlayer);
        }

        switch (_currentState)
        {
            case SkeletonControllerState.Idle:
                if (_currentStateTime >= _idleDuration)
                {
                    _walkDirection *= -1;
                    SetState(SkeletonControllerState.Walking);
                    break;
                }
                break;
            case SkeletonControllerState.Walking:
                if (_currentStateTime >= _idleDuration || CheckForFallOrWall())
                {
                    SetState(SkeletonControllerState.Idle);
                    break;
                }
                horizontalAxis = _walkDirection;
                break;
            case SkeletonControllerState.ChasingPlayer:
                if (!playerInSight)
                {
                    SetState(SkeletonControllerState.Idle);
                    break;
                }
                var playerDirection = GetPlayer().transform.position - transform.position;
                if (Mathf.Abs(playerDirection.x) > _AttackDistance) // out of attack distance, reset time;
                {
                    _walkDirection = (int)Mathf.Sign(playerDirection.x);
                    horizontalAxis = _walkDirection;
                    _currentStateTime = 0;
                }
                if (_currentStateTime >= _attackPrepTime) // if enough prep time within distance, attack
                {
                    parameters.Add(InputType.Attack, _walkDirection);
                    SetState(SkeletonControllerState.Attacking);
                }
                break;
            case SkeletonControllerState.Attacking:
                if (_currentStateTime >= _attackTime)
                {
                    SetState(SkeletonControllerState.ChasingPlayer);
                }
                break;

        }
        _currentStateTime += DeltaTime;
        if (horizontalAxis != 0)
        {
            parameters.Add(InputType.HorizontalAxis, horizontalAxis);
        }

        if (_damaged)
        {
            parameters.Add(InputType.DamageSourcePosition, _damageSourcePosition);
            _damaged = false;
            _damageSourcePosition = Vector2.zero;
        }

        if (!_isAlive)
        {
            parameters.Add(InputType.Dead, true);
        }

        _characterStateRunner.RunStates(parameters);
    }

    private void SetState(SkeletonControllerState state)
    {
        if (_currentState == state) return;
        _currentState = state;
        _currentStateTime = 0;
    }

    private bool CheckForFallOrWall()
    {
        var checkStart = transform.position + (Vector3)_collider2D.offset + _collider2D.size.x * _walkDirection * Vector3.right;
        var fall = Physics2D.Raycast(checkStart, Vector2.down, _collider2D.size.y * 0.6f, 1 << 7).collider == null;

        checkStart = transform.position + (Vector3)_collider2D.offset + _collider2D.size.y / 3 * Vector3.down;
        return fall || Physics2D.Raycast(checkStart, Vector2.right * _walkDirection, _collider2D.size.x, 1 << 7).collider != null;
    }


    BaseCharacter GetPlayer()
    {
        if (_playerList && _playerList.Objects.Count > 0)
        {
            return _playerList.Objects[0];
        }
        return null;
    }

    bool IsPlayerInSight()
    {
        var player = GetPlayer();
        if (player == null)
        {
            return false;
        }
        var sensorRect = new Rect((Vector2)transform.position + (_walkDirection < 0 ? Vector2.left * _senseDistance : Vector2.zero) + _senseHeight * Vector2.down / 2, new Vector2(_senseDistance, _senseHeight));
        return sensorRect.Contains(player.transform.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var sensorRect = new Rect((Vector2)transform.position + (_walkDirection < 0 ? Vector2.left * _senseDistance : Vector2.zero) + _senseHeight * Vector2.down / 2, new Vector2(_senseDistance, _senseHeight));
        Gizmos.DrawWireCube(
            sensorRect.center,
            sensorRect.size);
    }

    protected override void Die()
    {
        base.Die();
        for (int i = 0; i < 10; i++)
        {
            Instantiate(_pointParticlePrefab).transform.position = transform.position;
        }
    }
}
