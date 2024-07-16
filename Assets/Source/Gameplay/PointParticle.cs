using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class PointParticle : TimeboundMonoBehaviour
{

    [SerializeField]
    private ScriptableInt _scoreCounter;

    [SerializeField]
    private ScriptableTransformList _pointParticleList;

    [SerializeField]
    private float _startSpeed;

    [SerializeField]
    [Range(0, 10)]
    private float _lerpIntensity = 7;

    [SerializeField]
    private ParticleSystem _particleSystem;
    private Vector2 _currentVelocity;

    private float _particleAge = 0;

    private bool _consumed = false;

    void OnEnable()
    {
        _currentVelocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward) * Vector3.right * _startSpeed;
        _pointParticleList.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)_currentVelocity * DeltaTime;
        _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, _lerpIntensity * _particleAge * DeltaTime / _currentVelocity.magnitude);
        _particleAge += DeltaTime;
    }

    public void Consume()
    {
        if (_consumed) return;
        _scoreCounter.Value += 1;
        _consumed = true;
        var emissionComponent = _particleSystem.emission;
        emissionComponent.enabled = false;
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            Destroy(gameObject);
        });
    }

    void OnDisable()
    {
        _pointParticleList.Remove(transform);
    }

    public void PullTowards(Vector2 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, _lerpIntensity * _particleAge * DeltaTime / Vector2.Distance(transform.position, target));
    }

}
