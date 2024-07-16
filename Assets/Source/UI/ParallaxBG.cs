using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

[ExecuteInEditMode]
public class ParallaxBG : MonoBehaviour
{

    [SerializeField]
    float _parallaxPower;

    [SerializeField]
    Transform _followTarget;

    [SerializeField]
    private List<SpriteRenderer> _renderers;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_followTarget == null)
        {
            return;
        }
        for (int i = 0; i < _renderers.Count; i++)
        {
            var pos = _renderers[i].transform.position;
            var parallaxPos = _followTarget.position.x;
            pos.x = parallaxPos * 18f / 16 * _parallaxPower * i % 18 + parallaxPos; // BG width divided by Cam width
            _renderers[i].transform.position = pos;
        }
    }
}
