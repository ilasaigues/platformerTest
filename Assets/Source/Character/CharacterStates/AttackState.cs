using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AttackState : BaseCharacterState
{

    [Inject]
    private Animator _animator;

    [SerializeField]
    private BoxCollider2D _attackTrigger;


    [SerializeField]
    private float _attackDuration = 8f / 60;

    private float _totalDuration;
    private bool _isAttacking;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<CharacterStateRunner.CharacterStateInputType, object> controlParameters)
    {
        if (!_isAttacking && controlParameters.TryGetValue(CharacterStateRunner.CharacterStateInputType.Attack, out var _attackDirection))
        {
            _animator.SetTrigger("Attack");
            _isAttacking = true;
            var triggerOffset = _attackTrigger.offset;
            triggerOffset.x = Mathf.Abs(triggerOffset.x) * (int)_attackDirection;
            _attackTrigger.offset = triggerOffset;
            return false;
        }
        if (_isAttacking && (_totalDuration += deltaTime) < _attackDuration)
        {
            return false;
        }
        _attackTrigger.enabled = false;
        _isAttacking = false;
        _totalDuration = 0;
        return true;
    }

    public override void ResetState()
    {
        _attackTrigger.enabled = false;
        _isAttacking = false;
        _totalDuration = 0;
    }
}
