using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TakeDamageState : BaseCharacterState
{

    [SerializeField]
    private float _maxFlyTime = 1;

    private float _currentFlyTime = 0;

    private bool _flying;

    private Vector2 _origin;
    private Vector2 _target;
    private Vector2 _controlPoint;

    [Inject]
    private CharacterMovementController _movementController;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<CharacterStateRunner.CharacterStateInputType, object> controlParameters)
    {
        if (!_flying) // Not currently being knocked back, check for sources of damage
        {
            if (controlParameters.TryGetValue(CharacterStateRunner.CharacterStateInputType.DamageSourcePosition, out var damageSourcePositionObj))
            {
                var damageSourcePosition = (Vector2)damageSourcePositionObj;
                _flying = true;
                _origin = transform.position;
                if (damageSourcePosition.x > _origin.x) // Character is left of source, fly left
                {
                    _target = _origin + Vector2.left * 2 + Vector2.up;
                    _controlPoint = _origin + Vector2.left + Vector2.up;
                }
                else // character is right of source, fly right
                {
                    _target = _origin + Vector2.right * 2 + Vector2.up;
                    _controlPoint = _origin + Vector2.right + Vector2.up;
                }
                return false;
            }
            return true;
        }
        else // currently being knocked back, interpolate correspondingly
        {
            _currentFlyTime += deltaTime;

            var lerpValue = _currentFlyTime / _maxFlyTime;

            var pointA = Vector2.Lerp(_origin, _controlPoint, lerpValue);
            var pointB = Vector2.Lerp(_controlPoint, _target, lerpValue);

            var currentPoint = Vector2.Lerp(pointA, pointB, Mathf.Pow(lerpValue, 1f / 3));
            var delta = currentPoint - (Vector2)transform.position;

            _movementController.AddMovement(delta);

            if (lerpValue >= 1)
            {
                ResetState();
            }
            return false;
        }
    }

    public override void ResetState()
    {
        _flying = false;
        _currentFlyTime = 0;
        _origin = _target = _controlPoint = Vector2.zero;
    }
}
