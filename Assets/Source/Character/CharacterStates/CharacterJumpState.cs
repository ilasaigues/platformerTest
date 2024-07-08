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

    private float _currentVelocity = 0;

    private bool _isJumping;

    [Inject]
    CharacterMovementController _movementController;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<InputType, object> controlParameters)
    {
        if (!_isJumping && controlParameters.ContainsKey(InputType.JumpCommand))
        {
            _isJumping = true;
            _currentVelocity = _jumpPower;
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
            _currentVelocity += _jumpGravity * deltaTime;
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
        _isJumping = false;
    }
}
