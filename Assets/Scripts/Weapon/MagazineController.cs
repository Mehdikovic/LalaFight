using System;
using System.Collections;
using UnityEngine;


public class MagazineController : MonoBehaviour, IMagazineController, IOnObjectNotifier<Weapon>
{
    private Weapon _weapon = null;

    private int _currentMagazine = 0;
    private bool _isReloading = false;

    private Coroutine _reloadCoroutine;
    private IntObject _ammo = new IntObject() { Value = 500 };

    //EVENTS
    public event Action NoAmmunationAtInventory;
    public event Action Reloading;
    public event Action Reloaded;
    public event Action ReloadingCanceled;
    public event Action MagazineFull;

    //GETTERS AND SETTERS
    public int magazineSize => _weapon.magazieSize;

    public float reloadTime => _weapon.reloadTime;

    public int currentMagazine => _currentMagazine;

    public bool isReloading => _isReloading;

    //Unity CALLBACKS
    private void OnEnable()
    {
        _weapon.AttachMagazineController(this);
        _currentMagazine = _weapon.currentMagazine;
    }

    private void OnDisable()
    {
        _weapon.ClearMagazineController();
        _weapon.currentMagazine = _currentMagazine;
    }

    public void OnAwakeCalled(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void Reload()
    {
        if (_isReloading)
            return;
        if (_ammo.Value <= 0)
        {
            NoAmmunationAtInventory?.Invoke();
            return;
        }
        if (_currentMagazine == magazineSize)
        {
            MagazineFull?.Invoke();
            return;
        }

        _isReloading = true;
        Reloading?.Invoke();
        _reloadCoroutine = StartCoroutine(ReloadingWaitTime());
    }

    public void CancelReloading()
    {
        if (_isReloading == false)
            return;
        _isReloading = false;
        StopCoroutine(_reloadCoroutine);
        ReloadingCanceled?.Invoke();
    }

    private IEnumerator ReloadingWaitTime()
    {
        yield return new WaitForSeconds(reloadTime);

        var neededAmmo = magazineSize - _currentMagazine;
        if (neededAmmo >= _ammo.Value)
        {
            _currentMagazine += _ammo.Value;
            _ammo.Value = 0;
        }
        else if (neededAmmo < _ammo.Value)
        {
            _currentMagazine += neededAmmo;
            _ammo.Value -= neededAmmo;
        }

        _isReloading = false;
        Reloaded?.Invoke();
    }

    public void DecreaseAmmo()
    {
        --_currentMagazine;
    }

    public void SetCurrentMagazine(int amount)
    {
        _currentMagazine = Mathf.Clamp(amount, 0, magazineSize);
    }

    public bool ShootingAllowed()
    {
        return _currentMagazine > 0 && _isReloading == false;
    }
}