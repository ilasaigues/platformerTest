using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMover : MonoBehaviour
{
    [SerializeField]
    public Vector3 _speed;


    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime;
    }
}
