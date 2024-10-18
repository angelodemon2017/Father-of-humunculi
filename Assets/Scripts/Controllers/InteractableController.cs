using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// TODO REMOVE WAS TEST CLASS
/// </summary>
public class InteractableController : MonoBehaviour
{
    [SerializeField] private EnumFraction _fraction;
    [SerializeField] private List<InteractableController> _inRange = new();

    public EnumFraction Fraction => _fraction;

    private int _currentHP;
    public int CurrentHP 
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            OnUpdateHP?.Invoke();
            if (_currentHP > _maxHP)
            {
                _currentHP = _maxHP;
            }
            if (_currentHP < 0)
            {
                _currentHP = 0;
            }
        }
    }
    [SerializeField] private int _maxHP;
    public int MaxHP => _maxHP;
    [SerializeField] private int _HPregen;
    private float _timerRegen = 1;
    public bool IsDeath => CurrentHP <= 0;

    public Action OnUpdateHP;

    private void Awake()
    {
        CurrentHP = _maxHP;
    }

    private void Update()
    {
        Regeneration();
    }

    private void Regeneration()
    {
        if (!IsDeath && CurrentHP < _maxHP && _HPregen > 0)
        {
            _timerRegen -= Time.deltaTime;
            if (_timerRegen <= 0)
            {
                CurrentHP += _HPregen;
                _timerRegen = 1f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        _timerRegen = 3f;
    }

    public void Respawn()
    {
        CurrentHP = MaxHP;
        _timerRegen = 1f;
    }

    public InteractableController GetAvailableForAttack()
    {
        return _inRange.Count == 0 ? null : 
            _inRange.FirstOrDefault(x => x.TargetOk(this));
    }

    public bool TargetOk(InteractableController sc)
    {
        return Dict.TargetAttack[sc.Fraction].HasFlag(_fraction) && !IsDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        var sc = other.GetComponentInParent<InteractableController>();
        if (sc != null)
        {
            _inRange.Add(sc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var sc = other.GetComponentInParent<InteractableController>();
        if (sc != null && _inRange.Contains(sc))
        {
            _inRange.Remove(sc);
        }
    }
}