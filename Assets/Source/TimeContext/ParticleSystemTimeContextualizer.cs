using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTimeContextualizer : TimeContextualizer<ParticleSystem>
{
    protected override void SetActive(ParticleSystem component, bool paused)
    {
        if (paused)
        {
            component.Pause();
        }
        else
        {
            component.Play();
        }
    }

    protected override void SetSpeed(ParticleSystem component, float speed)
    {
        var mainComponent = component.main;
        mainComponent.simulationSpeed = speed;
    }
}
