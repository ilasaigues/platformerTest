using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using InputType = CharacterStateRunner.CharacterStateInputType;

[RequireComponent(typeof(CharacterMovementController))]
public class HorizontalMovementState : BaseCharacterState
{
    [SerializeField]
    private float _acceleration = 1;

    [SerializeField]
    private float _maxVelocity = 5;

    private float _horizontalVelocity = 0;

    [Inject]
    CharacterMovementController _movementController;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<InputType, object> controlParameters)
    {
        if (controlParameters.TryGetValue(InputType.HorizontalAxis, out var horizontalAxis))
        {
            _horizontalVelocity += (float)horizontalAxis * _acceleration * deltaTime;
            if (_horizontalVelocity > _maxVelocity || _horizontalVelocity < -_maxVelocity)
            {
                _horizontalVelocity = Mathf.Clamp(_horizontalVelocity, -_maxVelocity, _maxVelocity);
            }
        }
        else
        {
            var deceleration = _acceleration * Mathf.Sign(_horizontalVelocity) * deltaTime;
            if (Mathf.Abs(deceleration) > Mathf.Abs(_horizontalVelocity))
            {
                _horizontalVelocity = 0;
            }
            else
            {
                _horizontalVelocity -= deceleration;
            }
        }
        var wallHit = CheckForWall(deltaTime, (Vector2)controlParameters[InputType.HalfExtents]);
        if (wallHit.collider != null)
        {
            var hitPointVector = wallHit.point - (Vector2)transform.position;
            _horizontalVelocity = hitPointVector.magnitude - ((Vector2)controlParameters[InputType.HalfExtents]).x;
        }
        if (_horizontalVelocity != 0)
        {
            _movementController.AddMovement(_horizontalVelocity * deltaTime * Vector3.right);
        }
        return true;
    }

    public override void ResetState()
    {
        _horizontalVelocity = 0;
    }

    RaycastHit2D CheckForWall(float deltaTime, Vector2 halfExtents)
    {
        float distanceThisFrame = _horizontalVelocity * deltaTime;
        var sideVector = Vector2.right * distanceThisFrame;
        Vector2[] points = new Vector2[]{
            (Vector2)transform.position + sideVector + halfExtents.y * Vector2.up*0.9f,
            (Vector2)transform.position + sideVector - halfExtents.y * Vector2.up*0.9f,
        };

        foreach (var point in points)
        {
            var rch = Physics2D.Raycast(point, Vector2.right, halfExtents.x * Mathf.Sign(distanceThisFrame) + distanceThisFrame, 1 << 7);
            if (rch.collider != null)
            {
                return rch;
            }
        }
        return default;
    }
}
