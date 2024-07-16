using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{

    [SerializeField]
    private ScriptableEvent _VictoryEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_VictoryEvent != null && other.GetComponent<PlayerController>() != null)
        {
            _VictoryEvent.Raise();
        }
    }

}
