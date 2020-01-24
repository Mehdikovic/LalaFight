﻿using System;
using System.Collections;
using UnityEngine;


public class MagazineController : MonoBehaviour, IMagazineController
{
    [SerializeField] private int _magazineSize = 30;
    [SerializeField] private float _reloadTime = 1.3f;
	
	private int _currentMagazine = 0;
    private bool _isReloading = false;

	private Coroutine _reloadCoroutine;
	private IntObject _ammo = new IntObject() { Value = 500 };
	private Weapon _weapon;

	

	//EVENTS
	public event Action NoAmmunationAtInventory;
	public event Action Reloading;
	public event Action Reloaded;
	public event Action ReloadingCanceled;
	public event Action MagazineFull;

	//GETTERS AND SETTERS
	public int magazineSize => _magazineSize;

    public float reloadTime => _reloadTime;

    public int currentMagazine => _currentMagazine;

    public bool isReloading => _isReloading;


	private void Awake()
	{
		_weapon = GetComponent<Weapon>();
		
		_weapon.OnReloadRequested += OnReloadRequested;
		_weapon.OnFireLockRequested += OnFireLockRequested;
		_weapon.OnFireEnd += OnFireEnd;
		_weapon.OnWeaponUnloaded += OnWeaponUnloaded;

		//TODO: Remember we need to update this when UPGRADING happend
		_currentMagazine = _magazineSize;
	}

	private void OnWeaponUnloaded()
	{
		if (_isReloading == true)
			CancelReloading();
	}

	private void OnFireLockRequested()
	{
		_weapon.SetLockValue(LockType.Magazine, !ShootingAllowed());
	}

	private void OnFireEnd()
	{
		DecreaseAmmo();
	}

	private void OnReloadRequested()
	{
		Reload();
	}

	private bool ShootingAllowed()
    {
        return _currentMagazine > 0 && _isReloading == false;
    }

    private void Reload()
    {
		if (_isReloading)
			return;
		if (_ammo.Value <= 0)
		{
			NoAmmunationAtInventory?.Invoke();
			return;
		}
		if (_currentMagazine == _magazineSize)
		{
			MagazineFull?.Invoke();
			return;
		}

		_isReloading = true;
		Reloading?.Invoke();
		_reloadCoroutine = StartCoroutine(ReloadingWaitTime());
	}

	private void CancelReloading()
	{
		_isReloading = false;
		StopCoroutine(_reloadCoroutine);
		ReloadingCanceled?.Invoke();
	}

	private IEnumerator ReloadingWaitTime()
	{
		yield return new WaitForSeconds(_reloadTime);

		var neededAmmo = _magazineSize - _currentMagazine;
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

	private void DecreaseAmmo()
	{
		--_currentMagazine;
	}

	private void OnDestroy()
	{
		_weapon.SetLockValue(LockType.Magazine, false);
		
		_weapon.OnReloadRequested -= OnReloadRequested;
		_weapon.OnFireLockRequested -= OnFireLockRequested;
		_weapon.OnFireEnd -= OnFireEnd;
		_weapon.OnWeaponUnloaded -= OnWeaponUnloaded;
	}
}