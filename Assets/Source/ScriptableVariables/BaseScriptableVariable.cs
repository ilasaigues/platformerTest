using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseScriptableVariable : ScriptableObject
{
    [SerializeField]
    [TextArea(3, 6)]
    private string _developerDescription;

    public abstract object BaseValue { get; protected set; }
}
