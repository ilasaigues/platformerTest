using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField]
    private int _contactDamage = 1;

    [SerializeField]
    private LayerMask _damagedLayers;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & _damagedLayers) == 0)
        {
            return;
        }
        var character = other.GetComponent<BaseCharacter>();
        if (character != null)
        {
            character.TakeDamage(_contactDamage, transform.position);
        }
    }
}
