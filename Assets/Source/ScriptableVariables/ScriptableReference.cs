using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
[System.Serializable]
public abstract class BaseScriptableReference
{
    [SerializeField]
    protected int _usage = 0; // 0 = local, 1 = reference
}

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