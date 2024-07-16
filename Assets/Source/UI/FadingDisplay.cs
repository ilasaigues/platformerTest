using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadingDisplay : MonoBehaviour
{
    [SerializeField]
    private ScriptableTransformList _faderTransforms;

    [SerializeField]
    private float _minDistance = 3;

    [SerializeField]
    private float _maxDistance = 6;

    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _faderTransforms.Add(transform);
    }
    void OnDisable()
    {
        _faderTransforms.Remove(transform);
    }


    public void UpdateByDistance(Vector3 playerPosition)
    {
        var sqrDistance = (playerPosition - transform.position).sqrMagnitude;
        var power = Mathf.Clamp01(1 - (sqrDistance - Mathf.Pow(_minDistance, 2)) / (Mathf.Pow(_maxDistance, 2) - Mathf.Pow(_minDistance, 2)));
        _canvasGroup.alpha = power;
    }
}
