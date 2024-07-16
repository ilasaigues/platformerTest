using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
/// <summary>
/// This class should be used for any and all monobehaviours that depend on a variable TimeContext to adjust their speed or pause/unpause them
/// </summary>
public abstract class TimeboundMonoBehaviour : MonoBehaviour
{
    [SerializeField] protected ScriptableTimeContext _timeContext;

    protected float DeltaTime => _timeContext ? _timeContext.DeltaTime : Time.deltaTime;
    protected float FixedDeltaTime => _timeContext ? _timeContext.FixedDeltaTime : Time.fixedDeltaTime;

    private void Start()
    {
        if (_timeContext != null)
        {
            _timeContext.OnPause += OnPause;
            _timeContext.SubscribeToValueChanged(OnSpeedChanged);
        }
    }

    public void SetTimeContext(ScriptableTimeContext newTimeContext)
    {
        UnsubscribeFromTimeContext();
        _timeContext = newTimeContext;
        _timeContext.OnPause += OnPause;
    }

    private void UnsubscribeFromTimeContext()
    {
        if (_timeContext != null)
        {
            _timeContext.OnPause -= OnPause;
            _timeContext.UnSubscribeToValueChanged(OnSpeedChanged);

        }
    }

    void OnDestroy()
    {
        UnsubscribeFromTimeContext();
    }

    protected virtual void OnPause(bool pause)
    {
        // no op
    }

    protected virtual void OnSpeedChanged(float speed)
    {
        // no op
    }

}
