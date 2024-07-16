using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DeathState : BaseCharacterState
{
    [Inject]
    private Animator _animator;
    private bool _dead = false;
    public override bool ExecuteAndContinue(float deltaTime, Dictionary<CharacterStateRunner.CharacterStateInputType, object> controlParameters)
    {
        if (controlParameters.ContainsKey(CharacterStateRunner.CharacterStateInputType.Dead))
        {
            if (!_dead)
            {
                _dead = true;
                _animator.SetTrigger("Dead");

            }
        }
        return !controlParameters.ContainsKey(CharacterStateRunner.CharacterStateInputType.Dead);
    }

    public override void ResetState()
    {
    }
}
