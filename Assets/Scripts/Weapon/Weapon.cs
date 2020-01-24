using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LockType { Magazine, Equipment }

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _type = WeaponType.Pistol;
    [SerializeField] private string _modelName = "ex: m4a1";
    
    [Header("Properties")]
    [SerializeField] private int _damage = 4;
    [SerializeField] private float _bulletSpeed = 80f;
    [SerializeField] private float _accuracy = 1.0f;
    [SerializeField] private float _fireRate = 0.1f;
    
    [Header("Bullet")]
    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private Bullet _bulletPrefab = null;

    private float _nextShootingTime = 0f;
    
    private Transform _playerOwner = null;
    private AmmunationInventory _ammoInventory = null;

    private bool _isAnimating = false;

    private Dictionary<LockType, bool> _locks;

    //GETTERS AND SETTERS
    public Transform playerOwner => _playerOwner;
    public string modelName => _modelName;
    public int damage => _damage;
    public float bulletSpeed => _bulletSpeed;
    public WeaponType type => _type;
    public float accuracy => _accuracy;
    public bool isAnimating => _isAnimating;

    //EVENTS
    public event Action OnFireLockRequested;
    public event Action OnFireBegin;
    public event Action OnFireEnd;
    public event Action OnReloadRequested;
    public event Action OnWeaponLoaded;
    public event Action OnWeaponUnloaded;
    public event Action<Vector3, bool> OnCursorPositionReceived;

    // UNITY CALLBACKS
    protected virtual void Awake()
    {
        _locks = new Dictionary<LockType, bool>() {
                { LockType.Magazine, false },
                { LockType.Equipment, false },
        };

        //TODO: Create ScriptleObject for holding data and level-ups
        //SetProperties(currentWeaponData);
    }


    //CUSTOM METHODS
    protected abstract void HandleShootInputs();

    public virtual void HandleUpdateInputs()
    {
        if (Input.GetButtonDown("Reload"))
            OnReloadRequested?.Invoke();
        
        HandleShootInputs();
    }

    

    public void SetLockValue(LockType lockType, bool value)
    {
        _locks[lockType] = value;
    }

    private bool IsWeaponLocked()
    {
        if (OnFireLockRequested == null)
            return false;

        bool result = false;
        
        foreach (var item in _locks.Keys)
            result |= _locks[item];
        
        return result;
    }

    protected void CallOnFiredBeginEvent()
    {
        OnFireLockRequested?.Invoke();
        OnFireBegin?.Invoke();
    }
    protected void CallOnFiredEndEvent() => OnFireEnd?.Invoke();
    protected bool CanShoot() => IsWeaponLocked() == false && _isAnimating == false && Time.time > _nextShootingTime;
    protected void NextShootingTime() => _nextShootingTime = Time.time + _fireRate;
    
    public void SetShootCursorPosition(Vector3 hitPoint, bool aimOnEnemy)
    {
        OnCursorPositionReceived?.Invoke(hitPoint, aimOnEnemy);
    }

    public virtual void FastLoad(bool _isWeaponHided)
    {
        gameObject.SetActive(true);
        OnWeaponLoaded?.Invoke();
        gameObject.SetActive(!_isWeaponHided);
    }

    public virtual void FastUnload()
    {
        OnWeaponLoaded?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void OnWeaponLoad(bool _isWeaponHided)
    {
        if (_isAnimating == true) return;
        
        _isAnimating = true;
        gameObject.SetActive(true);
        
        OnWeaponLoaded?.Invoke();
        StartCoroutine(LoadAnimation(60, 0, true));
        
        gameObject.SetActive(!_isWeaponHided);
    }

    public virtual void OnWeaponUnload()
    {
        if (_isAnimating == true) return;
        
        _isAnimating = true;
        
        OnWeaponUnloaded?.Invoke();
        StartCoroutine(LoadAnimation(0, 60, false));
        
    }

    private IEnumerator LoadAnimation(float angleFrom, float angleTo, bool loaded)
    {
        //TODO: grab this 5 value from player stat
        float speed = 2;

        float percent = 0f;
        while(percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localEulerAngles = Vector3.right * Mathf.Lerp(angleFrom, angleTo, percent);
            yield return null;
        }

        _isAnimating = false;
        gameObject.SetActive(loaded);
    }

    public void SetOwner(Transform owner)
    {
        _playerOwner = owner;
    }

    public void SetAmmoInventory(AmmunationInventory ammunationInventory)
    {
        _ammoInventory = ammunationInventory;
    }

    protected void CreateBullet(float rotation = 0)
    {
        var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
        bullet.Initialize(this, rotation);
    }
}