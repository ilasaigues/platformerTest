using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InputType = CharacterStateRunner.CharacterStateInputType;
[RequireComponent(typeof(CharacterStateRunner))]
public class PlayerController : MonoBehaviour
{

    [Header("Shared Variables")]
    [SerializeField]
    float _halfHeight = 0.5f;
    [SerializeField]
    float _halfWidth = 0.5f;

    public IntReference MaxHealth;
    public ScriptableInt CharacterHealth;

    private CharacterStateRunner _characterStateRunner;

    private bool _damaged;
    private Vector2 _damageSourcePosition;


    // Start is called before the first frame update
    void Start()
    {
        _characterStateRunner = GetComponent<CharacterStateRunner>();
        CharacterHealth.Value = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        var parameters = new Dictionary<InputType, object>
        {
            { InputType.HalfExtents, new Vector2(_halfWidth, _halfHeight) }
        };

        var horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            parameters.Add(InputType.HorizontalAxis, horizontalAxis);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            parameters.Add(InputType.JumpCommand, true);
        }

        if (_damaged)
        {
            parameters.Add(InputType.DamageSourcePosition, _damageSourcePosition);
            _damaged = false;
            _damageSourcePosition = Vector2.zero;
        }

        _characterStateRunner.RunStates(parameters);
    }

    public void TakeDamage(int damage, Vector2 hazardPosition)
    {
        CharacterHealth.Value -= damage;
        _damaged = true;
        _damageSourcePosition = hazardPosition;
    }
}
