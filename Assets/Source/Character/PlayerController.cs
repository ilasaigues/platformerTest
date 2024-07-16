using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using InputType = CharacterStateRunner.CharacterStateInputType;
public class PlayerController : BaseCharacter
{

    [SerializeField]
    private ScriptableEvent _onPlayerDeath;

    [Inject]
    private SpriteRenderer _spriteRenderer;

    private int _remainingJumps = 0;

    // Update is called once per frame
    void Update()
    {
        var parameters = new Dictionary<InputType, object>
        {
            { InputType.HalfExtents, _collider2D.size/2 },
            { InputType.ColliderOffset, _collider2D.offset},
        };

        var horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            parameters.Add(InputType.HorizontalAxis, horizontalAxis);
        }

        if (_remainingJumps > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            _remainingJumps = 0;
            parameters.Add(InputType.JumpCommand, true);
        }

        if (_damaged)
        {
            parameters.Add(InputType.DamageSourcePosition, _damageSourcePosition);
            _damaged = false;
            _damageSourcePosition = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            parameters.Add(InputType.Attack, _spriteRenderer.flipX ? -1 : 1);
        }

        if (!_isAlive)
        {
            parameters.Add(InputType.Dead, true);
        }

        _characterStateRunner.RunStates(parameters);
    }

    protected override void Die()
    {
        base.Die();
        _onPlayerDeath?.Raise();
        UniTask.Void(async () =>
        {
            await UniTask.Delay(1000);
            _spriteRenderer.enabled = false;
        });
    }

    public void OnPlayerGrounded()
    {
        _remainingJumps = 1;
    }
}
