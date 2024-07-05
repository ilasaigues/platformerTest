using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ScriptableObject value container to avoid dependencies.
/// Objects can subscribe to this object's OnValueChanged function
/// or read it's value directly to know what it is, without knowing
/// who changed it or who handles changing it. It does open the door to
/// multiple objects being able to change it, but like a static variable,
/// it's up to the coding standards and etiquette to /not/ do that.
/// </summary>
/// <typeparam name="T">Type that the object will contain</typeparam>
[System.Serializable]
public abstract class ScriptableVariable<T> : BaseScriptableVariable, IEquatable<ScriptableVariable<T>>
{

    public override object BaseValue
    {
        get
        {
            return _value;
        }
        protected set
        {
            Value = (T)value;
        }
    }

    public T Value { get => _value; set => SetValue(value); }
    [SerializeField]
    private T _value;

    public virtual T InitialValue { get => _initialValue; set => _initialValue = value; }
    [SerializeField]
    private T _initialValue = default;

    public T OldValue { get => _oldValue; }
    [SerializeField]
    private T _oldValue;

    void OnEnable()
    {
        SetInitialValues();
        TriggerInitialEvents();
    }

    private void SetInitialValues()
    {
        _oldValue = _value;
        _value = InitialValue;
    }

    void TriggerInitialEvents()
    {
        OnValueChanged(_value);
        OnValueChangedWithHistory(_oldValue, _value);
    }

    public bool SetValue(T newValue)
    {
        var changeValue = newValue.Equals(_value);

        if (changeValue)
        {
            _oldValue = _value;
            _value = newValue;
            OnValueChanged(_value);
            OnValueChangedWithHistory(_oldValue, _value);
        }
        return changeValue;
    }

    public bool SetValue(ScriptableVariable<T> other)
    {
        return SetValue(other.Value);
    }

    public bool Equals(ScriptableVariable<T> other)
    {
        return other == this;
    }



    public delegate void ValueChangedDelegate(T newValue);
    public ValueChangedDelegate OnValueChanged
    {
        get
        {
            if (_onValueChanged == null) { _onValueChanged = (n) => { }; }
            return _onValueChanged;
        }
    }
    private event ValueChangedDelegate _onValueChanged;

    public delegate void ValueChangedWithHistoryDelegate(T oldValue, T newValue);
    public ValueChangedWithHistoryDelegate OnValueChangedWithHistory
    {
        get
        {
            if (_onValueChangedWithHistory == null) { _onValueChangedWithHistory = (o, n) => { }; }
            return _onValueChangedWithHistory;
        }
    }
    private event ValueChangedWithHistoryDelegate _onValueChangedWithHistory;
}




