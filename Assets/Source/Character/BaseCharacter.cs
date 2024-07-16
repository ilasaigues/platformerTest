using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BaseCharacter : TimeboundMonoBehaviour
{
    [SerializeField]
    protected IntReference _maxHealth;
    [SerializeField]
    protected IntReference _characterHealth;

    [SerializeField]
    private ScriptableCharacterList _characterList;


    protected bool _isAlive = true;



    protected bool _damaged = false;
    protected Vector2 _damageSourcePosition;

    [Inject]
    protected Animator _animator;

    [Inject]
    protected CharacterStateRunner _characterStateRunner;

    [Inject]
    protected BoxCollider2D _collider2D;

    protected virtual void Start()
    {
        _characterHealth.Value = _maxHealth.Value;
        if (_characterList)
        {
            _characterList.Add(this);
        }
    }

    public virtual void TakeDamage(int damage, Vector2 hazardPosition)
    {
        _characterHealth.Value = Mathf.Max(0, _characterHealth - damage);

        if (_characterHealth.Value <= 0)
        {
            if (_isAlive)
            {
                Die();
            }
            return;
        }
        _damaged = true;
        _damageSourcePosition = hazardPosition;
        _animator.SetTrigger("TakeDamage");
    }

    protected virtual void Die()
    {
        _isAlive = false;
        if (_characterList)
        {
            _characterList.Remove(this);
        }
    }

    void OnDestroy()
    {
        if (_characterList)
        {
            _characterList.Remove(this);
        }
    }

}
