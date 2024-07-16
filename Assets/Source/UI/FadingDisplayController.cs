using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadingDisplayController : MonoBehaviour
{
    [SerializeField]
    private ScriptableTransformList _faderTransforms;

    // Update is called once per frame
    void Update()
    {
        foreach (var fader in _faderTransforms.Objects.Select(t => t.GetComponent<FadingDisplay>()))
        {
            if (fader != null)
            {
                fader.UpdateByDistance(transform.position);
            }
        }
    }
}
