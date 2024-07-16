using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptables/Time Context", fileName = "newTimeContext")]
public class ScriptableTimeContext : ScriptableFloat, ITimeContext
{

    [SerializeField]
    private bool _paused;

    public bool Paused => _paused;

    public float DeltaTime => _paused ? 0 : Value * Time.deltaTime;

    public float FixedDeltaTime => _paused ? 0 : Value * Time.fixedDeltaTime;

    public System.Action<bool> OnPause = new System.Action<bool>(p => { });

    public void SetDeltaTimeMultiplier(float multiplier)
    {
        Value = multiplier;
    }

    public void SetFixedDeltaTimeMultiplier(float multiplier)
    {
        Value = multiplier;
    }

    public void TogglePause()
    {
        SetPause(_paused);
    }

    public void SetPause(bool pause)
    {
        if (_paused == pause) return;
        else
        {
            _paused = pause;
            OnPause(pause);
        }
    }
}
