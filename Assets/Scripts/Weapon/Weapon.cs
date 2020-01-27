﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LockType { Magazine, Equipment }

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _modelName = "ex: m4a1";
    [SerializeField] private WeaponType _type = WeaponType.Pistol;
    [SerializeField] private BulletType _bulletType = BulletType.Medium;
    
    [Header("Bullet")]
    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private Bullet _bulletPrefab = null;

    [Header("Weapon Stat[Single | Multiple]")]
    [SerializeField] protected WeaponStats _stats = null;
    
    private float _nextShootingTime = 0f;
    private bool _isAnimating = false;
    private Dictionary<LockType, bool> _locks;

    private Transform _playerOwner = null;
    private AmmunationInventory _ammoInventory = null;
    

    //TODO: add equipment manager to modifying the stats' values
    //GETTERS AND SETTERS
    public string modelName => _modelName;
    public WeaponType type => _type;
    public BulletType bulletType => _bulletType;
    public Transform playerOwner => _playerOwner;

    public float accuracy => _stats.accuracy.value;
    public int damage => _stats.damage.value;
    public float bulletSpeed => _stats.bulletSpeed.value;
    public float fireRate => _stats.fireRate.value;
    public int magazieSize => _stats.magazineSize.value;
    public float reloadTime => _stats.reloadTime.value;
    public float scopeRange => _stats.scopeRange.value;


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

    //TODO: delete this test
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var text = GameObject.FindGameObjectWithTag("TextMeshPro").GetComponent<UnityEngine.UI.Text>();
            text.text = _stats.ToString();
        }
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

    protected void RaiseOnFiredBeginEvent()
    {
        OnFireLockRequested?.Invoke();
        OnFireBegin?.Invoke();
    }
    protected void RaiseOnFiredEndEvent() => OnFireEnd?.Invoke();
    protected bool CanShoot() => IsWeaponLocked() == false && _isAnimating == false && Time.time > _nextShootingTime;
    protected void NextShootingTime() => _nextShootingTime = Time.time + fireRate;
    
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