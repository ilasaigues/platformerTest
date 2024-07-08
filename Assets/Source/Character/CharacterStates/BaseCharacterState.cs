using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InputType = CharacterStateRunner.CharacterStateInputType;

public abstract class BaseCharacterState : MonoBehaviour
{
    [SerializeField]
    protected BaseCharacterState[] _whitelistedStates;

    public BaseCharacterState[] WhitelistedStates => (BaseCharacterState[])_whitelistedStates.Clone();

    public abstract bool ExecuteAndContinue(float deltaTime, Dictionary<InputType, object> controlParameters);

    public abstract void ResetState();
}
