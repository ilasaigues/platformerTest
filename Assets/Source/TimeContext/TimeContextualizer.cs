using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class TimeContextualizer<T> : TimeboundMonoBehaviour where T : Component
{
    private T _component;

    protected virtual void OnEnable()
    {
        _component = GetComponent<T>();
    }

    protected override void OnSpeedChanged(float speed)
    {
        SetSpeed(_component, speed);
    }
    protected override void OnPause(bool pause)
    {
        SetActive(_component, pause);
    }


    protected abstract void SetSpeed(T component, float speed);

    protected abstract void SetActive(T component, bool paused);

}