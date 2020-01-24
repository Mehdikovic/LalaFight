using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth = 0;
    private bool _isDead = false;

    //GETTERS AND SETTERS
    public int MaxHealth {
        get => _maxHealth;
        set {
            _maxHealth = value;
            _currentHealth = _maxHealth;
        }
    }
    public int RemainingHealthPercent => Mathf.CeilToInt(_currentHealth * 100 / _maxHealth);
    public bool IsDead => _isDead;
    public int CurrentHealth => _currentHealth;

    //EVENTS
    public event Action Dead;
    public event Action<Vector3, Vector3> OnFinalDamage;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int bulletDamage, Vector3 hitPosition, Vector3 hitDirection)
    {
        _currentHealth -= bulletDamage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        if (_currentHealth == 0 && _isDead == false)
        {
            OnFinalDamage?.Invoke(hitPosition, hitDirection);
            Die();
        }
    }

    [ContextMenu("Kill The Living Entity")]
    private void Die()
    {
        _isDead = true;
        Dead?.Invoke();
        Destroy(gameObject);
    }
}
