using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event listener tied to the <see cref="ScriptableEvent"/> class. Subscription and desubscription happen on OnEnable and OnDisable,
/// triggered during instantiation and destruction, respectively.
/// </summary>
public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField]
    private ScriptableEvent _event;
    public UnityEvent Response;

    void OnEnable()
    {
        if (_event != null)
        {
            _event.RegisterListener(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void OnDisable()
    {
        if (_event != null)
        {
            _event.UnregisterListener(this);
        }
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
