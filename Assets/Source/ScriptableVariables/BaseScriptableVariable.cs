using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base type for <see cref="ScriptableVariable"/>, containing only developer notes
/// and an <see cref="object"/> type BaseValue later cast into the corresponding type.
/// </summary>
[System.Serializable]
public abstract class BaseScriptableVariable : ScriptableObject
{
    [SerializeField]
    [TextArea(3, 6)]
    private string _developerDescription;

    public abstract object BaseValue { get; protected set; }
}