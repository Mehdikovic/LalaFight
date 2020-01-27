using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AmmunationInventory))]
public class WeaponManager : MonoBehaviour
{
    [Range(1, 4)]
    [SerializeField] private int _allowedWeaponNumber = 3;
    [SerializeField] private Transform _weaponHolder = null;
    
    [Header("Starting Guns")]
    [SerializeField] private Weapon _pistolStartGun = null;
    [SerializeField] private Weapon[] _otherStartGun = null;


    private List<Weapon> _weapons = new List<Weapon>();
    private int _currentWeaponIndex = 0;
    private AmmunationInventory _ammunationInventory;
    private bool _isWeaponHided = false;

    public event Action WeaponSwapped;
    public event Action WeaponAdded;
    public event Action<bool> WeaponHideStateChanged;
    
    public Weapon currentWeapon => _weapons[_currentWeaponIndex];
    public bool isWeaponHided => _isWeaponHided;

    private void Awake()
    {
        _ammunationInventory = GetComponent<AmmunationInventory>();
        _ammunationInventory.Init();
        
        if (_pistolStartGun != null)
        {
            var weapon = Instantiate(_pistolStartGun, _weaponHolder.position, _weaponHolder.rotation, _weaponHolder);
            weapon.SetOwner(transform);
            weapon.SetAmmoInventory(_ammunationInventory);
            
            _weapons.Add(weapon);
            weapon.FastLoad(_isWeaponHided);
            WeaponAdded?.Invoke();
        }
        
        if (_otherStartGun != null && _otherStartGun.Length > 0)
        {
            for (int i = 0; i < _otherStartGun.Length; i++)
            {
                if (_weapons.Count >= _allowedWeaponNumber)
                    break;
                if (_otherStartGun[i].type == WeaponType.Pistol)
                    continue;
                var weapon = Instantiate(_otherStartGun[i], _weaponHolder.position, _weaponHolder.rotation, _weaponHolder);
                weapon.SetOwner(transform);
                weapon.SetAmmoInventory(_ammunationInventory);

                _weapons.Add(weapon);
                weapon.FastUnload();
                WeaponAdded?.Invoke();
            }
            
        }
    }

    public void SetCursorPositionAndLookAt(Vector3 hitPoint, float length, bool aimOnEnemy)
    {
        currentWeapon.SetShootCursorPosition(hitPoint, aimOnEnemy);
        if (length > 1)
            _weaponHolder.LookAt(hitPoint);
    }

    public void SwapWeapon(int changeIndex)
    {
        if (currentWeapon.isAnimating) return;
        
        if (_weapons.Count > 1 && _isWeaponHided == false)
        {
            var oldCurrentWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex += changeIndex;
            //_currentWeaponIndex = Mathf.Clamp(_currentWeaponIndex, 0, _weapons.Count - 1);
            _currentWeaponIndex = _currentWeaponIndex == _weapons.Count ? 0 : _currentWeaponIndex;
            _currentWeaponIndex = _currentWeaponIndex == -1 ? _weapons.Count - 1 : _currentWeaponIndex;
            if (oldCurrentWeaponIndex == _currentWeaponIndex)
                return;
            _weapons[oldCurrentWeaponIndex].OnWeaponUnload();
            _weapons[_currentWeaponIndex].OnWeaponLoad(_isWeaponHided);
            WeaponSwapped?.Invoke();
        }
    }

    public float GetWeaponActiveRange() => currentWeapon.scopeRange;

    public void ToggleWeaponEnable()
    {
        if (_isWeaponHided == false)
            HideWeapon();
        else
            ShowWeapon();
    }

    public void ShowWeapon()
    {
        if (_isWeaponHided == false || currentWeapon.isAnimating == true) return;
        
        _isWeaponHided = false;
        currentWeapon.OnWeaponLoad(_isWeaponHided);
        WeaponHideStateChanged?.Invoke(_isWeaponHided);
    }

    public void HideWeapon()
    {
        if (_isWeaponHided == true || currentWeapon.isAnimating == true) return;

        _isWeaponHided = true;
        currentWeapon.OnWeaponUnload();
        WeaponHideStateChanged?.Invoke(_isWeaponHided);
    }

    public void CurrentWeaponTick()
    {
        if (_isWeaponHided == true)
            return;
        currentWeapon.HandleUpdateInputs();
    }

    public Weapon AddWeapon(Weapon newWeapon)
    {
        newWeapon.FastUnload();

        if (newWeapon.type == WeaponType.Pistol)
        {
            var oldWeapon = _weapons[0];
            oldWeapon.OnWeaponUnload();
            SyncWeaponWithHolder(newWeapon);
            
            _weapons[0] = newWeapon;
            newWeapon.SetOwner(transform);
            newWeapon.SetAmmoInventory(_ammunationInventory);
            
            if (_currentWeaponIndex == 0)
            {
                _weapons[0].OnWeaponLoad(_isWeaponHided);
            }
            WeaponAdded?.Invoke();
            return oldWeapon;
        }

        if (_weapons.Count >= _allowedWeaponNumber)
        {
            if (_currentWeaponIndex == 0)
            {
                return newWeapon;
            }
            var oldWeapon = _weapons[_currentWeaponIndex];
            oldWeapon.OnWeaponUnload();
            SyncWeaponWithHolder(newWeapon);
            _weapons[_currentWeaponIndex] = newWeapon;
            _weapons[_currentWeaponIndex].OnWeaponLoad(_isWeaponHided);
            newWeapon.SetOwner(transform);
            newWeapon.SetAmmoInventory(_ammunationInventory);
            WeaponAdded?.Invoke();
            return oldWeapon;
        }

        _weapons.Add(newWeapon);
        newWeapon.SetOwner(transform);
        newWeapon.SetAmmoInventory(_ammunationInventory);
        WeaponAdded?.Invoke();
        return null;
    }

    private void SyncWeaponWithHolder(Weapon newWeapon)
    {
        newWeapon.transform.position = _weaponHolder.position;
        newWeapon.transform.rotation = _weaponHolder.rotation;
        newWeapon.transform.parent = _weaponHolder;
    }
}
