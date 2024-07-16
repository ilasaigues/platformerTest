using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollector : MonoBehaviour
{
    [SerializeField]
    private ScriptableTransformList _pointParticleList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_pointParticleList)
        {
            for (int i = _pointParticleList.Objects.Count - 1; i >= 0; i--)
            {
                var pointParticle = _pointParticleList.Objects[i];
                pointParticle.GetComponent<PointParticle>().PullTowards(transform.position);
                if (Vector2.Distance(pointParticle.position, transform.position) <= 0.1f)
                {
                    pointParticle.GetComponent<PointParticle>().Consume();
                }
            }
        }
    }
}
