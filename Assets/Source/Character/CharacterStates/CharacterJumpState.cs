using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using InputType = CharacterStateRunner.CharacterStateInputType;

public class CharacterJumpState : BaseCharacterState
{

    [SerializeField]
    private float _jumpPower = 5;

    [SerializeField]
    private float _jumpGravity;

    [SerializeField]
    private float _peakGravity;

    private float _currentVelocity = 0;

    private bool _isJumping;

    private bool _reachedPeak;

    [Inject]
    CharacterMovementController _movementController;

    [Inject]
    Animator _animator;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<InputType, object> controlParameters)
    {
        if (!_isJumping && controlParameters.ContainsKey(InputType.JumpCommand))
        {
            _animator.SetBool("Grounded", false);
            _animator.SetBool("Jump", true);
            _isJumping = true;
            _currentVelocity = _jumpPower;
        }

        if (!_reachedPeak && _currentVelocity <= 2)
        {
            _animator.SetBool("Jump", false);
            _animator.SetBool("BeginJumpPeak", true);
            _reachedPeak = true;
        }

        if (_currentVelocity <= 0)
        {
            ResetState();
        }

        if (_isJumping)
        {
            var rch = CheckForCeiling(deltaTime, (Vector2)controlParameters[InputType.HalfExtents]);
            if (rch.collider != null)
            {
                ResetState();
                return true;
            }
            _movementController.AddMovement(Vector2.up * _currentVelocity * deltaTime);
            _currentVelocity += (_reachedPeak ? _peakGravity : _jumpGravity) * deltaTime;
            return false;
        }

        return true;
    }

    RaycastHit2D CheckForCeiling(float deltaTime, Vector2 halfExtents)
    {
        float distanceThisFrame = _currentVelocity * deltaTime;
        RaycastHit2D[] results = new RaycastHit2D[2];
        if (Physics2D.RaycastNonAlloc(transform.position, Vector2.up, results, halfExtents.y + distanceThisFrame, 1 << 7) > 0)
        {
            foreach (var hit in results)
            {
                if (hit)
                {
                    return hit;
                }
            }
        }
        return default;
    }

    public override void ResetState()
    {
        _currentVelocity = 0;
        _isJumping = _reachedPeak = false;
    }
}
