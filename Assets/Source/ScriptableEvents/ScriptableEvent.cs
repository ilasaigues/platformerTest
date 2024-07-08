using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serialized event object that allows for an uncoupled observer pattern, where the observable raises the serialized event
/// and the observers simply listen for any calls to the event itself. The observables and the observers don't even know about each other.
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Event", fileName = "New Scriptable Event")]
public class ScriptableEvent : ScriptableObject
{
    [SerializeField]
    [TextArea(3, 6)]
    private string _developerDescription;

    private List<ScriptableEventListener> _listeners = new List<ScriptableEventListener>();
    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(ScriptableEventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnregisterListener(ScriptableEventListener listener)
    {
        _listeners.Remove(listener);
    }

    void OnEnable()
    {
        Debug.Log("Enable");
    }

    void OnDisable()
    {
        Debug.Log("Disable");
    }
}
