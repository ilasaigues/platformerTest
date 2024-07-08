using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterMovementController))]
public class CharacterStateRunner : TimeboundMonoBehaviour
{
    public enum CharacterStateInputType
    {
        HalfExtents,
        HorizontalAxis,
        JumpCommand,
        DamageSourcePosition,
    }

    [Inject]
    private CharacterMovementController _movementController;

    [Header("States")]
    [SerializeField]
    private BaseCharacterState[] baseCharacterStates;

    void Start()
    {
        foreach (var state in baseCharacterStates)
        {
            state.ResetState();
        }
    }

    public void RunStates(Dictionary<CharacterStateInputType, object> parameters)
    {
        bool overridden = false;
        List<BaseCharacterState> overrideWhiteList = new();
        for (int i = 0; i < baseCharacterStates.Length; i++)
        {
            BaseCharacterState currentState = baseCharacterStates[i];
            if (!overridden || overrideWhiteList.Contains(currentState))
            {
                overridden |= !currentState.ExecuteAndContinue(DeltaTime, parameters);
                if (overridden)
                {
                    overrideWhiteList.AddRange(currentState.WhitelistedStates);
                }
            }
            else
            {
                currentState.ResetState();
            }
        }
        _movementController.ApplyMovement();
    }
}
