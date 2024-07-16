using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleSpriteMatcher : MonoBehaviour
{
    [Inject]
    private SpriteRenderer _spriteRenderer;

    private ParticleSystem.ShapeModule _shapeModule;

    void Start()
    {
        _shapeModule = GetComponent<ParticleSystem>().shape;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(_spriteRenderer.flipX ? 180 : 0, Vector3.up);
        _shapeModule.sprite = _spriteRenderer.sprite;

    }
}
