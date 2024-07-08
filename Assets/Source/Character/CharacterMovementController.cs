using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    Vector2 _delta;
    public void AddMovement(Vector2 delta)
    {
        _delta += delta;
    }

    public void ApplyMovement()
    {
        transform.position += (Vector3)_delta;
        _delta = Vector2.zero;
    }
}
