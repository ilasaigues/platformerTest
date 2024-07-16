using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LookDirectionState : BaseCharacterState
{

    [Inject]
    private SpriteRenderer _renderer;

    public override bool ExecuteAndContinue(float deltaTime, Dictionary<CharacterStateRunner.CharacterStateInputType, object> controlParameters)
    {
        if (controlParameters.TryGetValue(CharacterStateRunner.CharacterStateInputType.HorizontalAxis, out var horizontalAxis))
        {
            if ((float)horizontalAxis != 0)
            {
                _renderer.flipX = (float)horizontalAxis < 0;
            }
        }
        return true;
    }

    public override void ResetState()
    {
        // no op
    }
}
