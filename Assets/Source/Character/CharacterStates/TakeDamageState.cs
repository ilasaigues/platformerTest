using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TakeDamageState : BaseCharacterState
{

    [SerializeField]
    private float _maxFlyTime = 1;
    [SerializeField]
    private Vector2 _targetOffset = Vector2.left * 2 + Vector2.up;
    [SerializeField]
    private Vector2 _controlPointOffset = Vector2.left + Vector2.up;

    [SerializeField]
    [Range(0, 1)]
    private float _flightFactor = 1f / 3;

    private float _currentFlyTime = 0;

    private bool _flying;

    private Vector2 _origin;
    private Vector2 _target;
    private Vector2 _controlPoint;

    [Inject]
    private CharacterMovementController _movementController;

    [Inject]
    private Animator _animator;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<CharacterStateRunner.CharacterStateInputType, object> controlParameters)
    {
        if (!_flying) // Not currently being knocked back, check for sources of damage
        {
            if (controlParameters.TryGetValue(CharacterStateRunner.CharacterStateInputType.DamageSourcePosition, out var damageSourcePositionObj))
            {
                var damageSourcePosition = (Vector2)damageSourcePositionObj;
                _flying = true;
                _animator.SetBool("Grounded", false);
                _origin = transform.position;
                if (damageSourcePosition.x < _origin.x) // Character is right of source, fly right
                {
                    _target = _origin + Vector2.Reflect(_targetOffset, Vector2.right);
                    _controlPoint = _origin + Vector2.Reflect(_controlPointOffset, Vector2.right);
                }
                else // character is left of source, fly left
                {
                    _target = _origin + _targetOffset;
                    _controlPoint = _origin + _controlPointOffset;
                }
                return false;
            }
            return true;
        }
        else // currently being knocked back, interpolate correspondingly
        {
            _currentFlyTime += deltaTime;

            var lerpValue = Mathf.Pow(_currentFlyTime / _maxFlyTime, _flightFactor);

            var currentPoint = EvaluateCubicBezier(_origin, _target, _controlPoint, lerpValue);

            var delta = currentPoint - (Vector2)transform.position;

            _movementController.AddMovement(delta);

            if (lerpValue >= 1)
            {
                ResetState();
            }
            return false;
        }
    }

    private Vector2 EvaluateCubicBezier(Vector2 start, Vector2 end, Vector2 controlPoint, float value)
    {
        var pointA = Vector2.Lerp(start, controlPoint, value);
        var pointB = Vector2.Lerp(controlPoint, end, value);
        return Vector2.Lerp(pointA, pointB, value);
    }

    public override void ResetState()
    {
        _flying = false;
        _currentFlyTime = 0;
        _origin = _target = _controlPoint = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var origin = (Vector2)transform.position;
        var target = origin + _targetOffset;
        var controlPoint = origin + _controlPointOffset;

        Gizmos.DrawWireSphere(controlPoint, 0.05f);
        Gizmos.DrawLine(controlPoint, target);
        Gizmos.DrawLine(controlPoint, origin);

        Gizmos.color = Color.red;
        List<Vector2> points = new()
        {
            origin,
        };

        int totalSteps = 20;

        for (int i = 1; i <= totalSteps; i++)
        {
            var lerpValue = Mathf.Pow(i * 1f / totalSteps, _flightFactor);
            points.Add(EvaluateCubicBezier(origin, target, controlPoint, lerpValue));
        }

        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawWireSphere(points[i], 0.05f);
            if (i < points.Count - 1)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }

    }
}
