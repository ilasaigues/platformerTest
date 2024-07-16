using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimatorTimeContextualizer : TimeContextualizer<Animator>
{

    private float _prevSpeed;
    protected override void SetActive(Animator component, bool paused)
    {
        if (paused && component.speed != 0)
        {
            _prevSpeed = component.speed;
            component.speed = 0;
        }
        if (!paused && _prevSpeed != 0)
        {
            component.speed = _prevSpeed;
        }
    }

    protected override void SetSpeed(Animator component, float speed)
    {
        component.speed = speed;
    }
}
