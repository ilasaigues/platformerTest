using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


/// <summary>
/// Base object for the ScriptableReference object, with only different usage data in it.
/// </summary>
[System.Serializable]
public abstract class BaseScriptableReference
{
    [SerializeField]
    protected int _usage = 0; // 0 = local, 1 = reference
}

/// <summary>
/// Wrapper for <see cref="ScriptableVariable"/> type objects, with an option to either use a local variable,
/// or a scriptable variable reference.
/// </summary>
/// <typeparam name="T">The type of variable this object returns</typeparam>
/// <typeparam name="U">The type of <see cref="ScriptableVariable"/> supported for the type <see cref="T"/></typeparam>
[System.Serializable]
public abstract class ScriptableReference<T, U> : BaseScriptableReference where U : ScriptableVariable<T>
{
    [SerializeField]
    private U _reference;

    [SerializeField]
    private T _localValue;

    public T Value
    {
        get { return _usage == 0 || _reference == null ? _localValue : _reference.Value; }
        set
        {
            if (_usage == 0 || _reference == null)
            {
                _localValue = value;
            }
            else
            {
                _reference.Value = value;
            }
        }
    }

    public static implicit operator T(ScriptableReference<T, U> reference)
    {
        return reference.Value;
    }
}

[System.Serializable]
public class FloatReference : ScriptableReference<float, ScriptableFloat> { }

[System.Serializable]
public class BoolReference : ScriptableReference<bool, ScriptableBool> { }

[System.Serializable]
public class IntReference : ScriptableReference<int, ScriptableInt> { }