using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth = 0;
    private bool _isDead = false;

    //GETTERS AND SETTERS
    public int maxHealth {
        get => _maxHealth;
        set {
            _maxHealth = value;
            _currentHealth = _maxHealth;
        }
    }
    public int remainingHealthPercent => Mathf.CeilToInt(_currentHealth * 100 / _maxHealth);
    public bool isDead => _isDead;
    public int currentHealth => _currentHealth;

    //EVENTS
    public event Action OnDeath;
    public event Action<Vector3, Vector3> OnFinalDamage;

    protected virtual void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int bulletDamage, Vector3 hitPosition, Vector3 hitDirection)
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
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}