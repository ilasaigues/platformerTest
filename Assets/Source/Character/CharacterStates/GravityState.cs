using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using InputType = CharacterStateRunner.CharacterStateInputType;

public class GravityState : BaseCharacterState
{
    [SerializeField]
    private float _gravity;

    [SerializeField]
    private ScriptableEvent _onCharacterGrounded;

    private float _currentVelocity;

    private bool _grounded = false;

    [Inject]
    CharacterMovementController _movementController;

    [Inject]
    Animator _animator;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<InputType, object> controlParameters)
    {
        Vector2 halfExtents = (Vector2)controlParameters[InputType.HalfExtents];
        Vector2 colliderOffset = (Vector2)controlParameters[InputType.ColliderOffset];
        if (_grounded)
        {
            _currentVelocity = 0;
            if (CheckForGround(deltaTime, colliderOffset, halfExtents).collider == null)
            {
                _grounded = false;
                _animator.SetBool("Grounded", false);
            }
        }
        else
        {
            RaycastHit2D rch = default;
            if (_currentVelocity < 0)
            {
                rch = CheckForGround(deltaTime, colliderOffset, halfExtents);
            }
            if (rch.collider != null)
            {
                _animator.SetBool("BeginJumpPeak", false);
                _animator.SetBool("Grounded", true);
                _grounded = true;
                _onCharacterGrounded?.Raise();
                _currentVelocity = 0;
                var distanceToGround = Mathf.Abs(rch.point.y - (transform.position.y + colliderOffset.y)) - halfExtents.y;
                _movementController.AddMovement(distanceToGround * Vector2.down);
            }
            else
            {
                _movementController.AddMovement(_currentVelocity * deltaTime * Vector2.up);
                _currentVelocity += _gravity * deltaTime;
            }
        }
        return true;
    }

    public override void ResetState()
    {
        _currentVelocity = 0;
        _grounded = false;
    }

    RaycastHit2D CheckForGround(float deltaTime, Vector2 colliderOffset, Vector2 halfExtents)
    {
        float distanceThisFrame = Mathf.Abs(_currentVelocity * deltaTime);
        var downVector = Vector2.down * distanceThisFrame;
        Vector2[] points = new Vector2[]{
            (Vector2)transform.position + colliderOffset + downVector + 0.9f * halfExtents.x * Vector2.right,
            (Vector2)transform.position + colliderOffset + downVector - 0.9f * halfExtents.x * Vector2.right,
        };

        if (_grounded)
        {
            foreach (var point in points)
            {
                var rch = Physics2D.Raycast(point, Vector2.down, halfExtents.y + 0.1f, 1 << 7);
                if (rch.collider != null)
                {
                    return rch;
                }
            }
            return default;
        }
        else
        {
            foreach (var point in points)
            {
                var rch = Physics2D.Raycast(point, Vector2.down, halfExtents.y + distanceThisFrame, 1 << 7);
                if (rch.collider != null)
                {
                    return rch;
                }
            }
        }
        return default;
    }
}
