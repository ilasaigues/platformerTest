using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    private GameObject _target;


    void Awake()
    {
        transform.parent.SetParent(null);
        DontDestroyOnLoad(transform.parent);
    }

    void Update()
    {
        _target = EventSystem.current.currentSelectedGameObject;
        if (!LeanTween.isTweening(gameObject))
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        var selectedRect = _target?.GetComponent<RectTransform>();
        var button = _target?.GetComponent<Button>();
        if (button == null || !button.interactable)
        {
            selectedRect = null;
        }

        var targetPosition = selectedRect != null ? selectedRect.transform.TransformPoint(Vector2.zero) : Vector3.up * 10000;
        var targetSize = selectedRect != null ? selectedRect.rect.size : Vector2.one;


        transform.LeanMove(targetPosition, 0.2f).setEaseOutCubic();
        GetComponent<RectTransform>().LeanSize(targetSize, 0.2f).setEaseOutCubic();
    }
}
