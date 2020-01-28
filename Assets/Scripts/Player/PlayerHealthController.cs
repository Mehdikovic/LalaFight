using UnityEngine;

public class PlayerHealthController : HealthController
{
    [SerializeField] private int _maxShield = 10;
    int _currentShield = 0;

    public int maxShield {
        get => _maxShield;
        set {
            _maxShield = value;
            _currentShield = _maxShield;
        }
    }
    public int remainingShieldPercent => Mathf.CeilToInt(_currentShield * 100 / _maxShield);
    public int currentShield => _currentShield;


    protected override void Awake()
    {
        base.Awake();
        _currentShield = _maxShield;
    }


    public override void TakeDamage(int bulletDamage, Vector3 hitPosition, Vector3 hitDirection)
    {
        if (_currentShield == 0)
        {
            base.TakeDamage(bulletDamage, hitPosition, hitDirection);
            return;
        }
        
        _currentShield -= bulletDamage;
        
        if (_currentShield < 0)
            base.TakeDamage(Mathf.Abs(_currentShield), hitPosition, hitDirection);
        
        _currentShield = Mathf.Clamp(_currentShield, 0, _maxShield);
        
    }
}
