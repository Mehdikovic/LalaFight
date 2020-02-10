using System;
using System.Collections;
using UnityEngine;


namespace LalaFight
{
    public class MagazineController : MonoBehaviour, IMagazineController, IOnObjectNotifier<Weapon>
    {
        private Weapon _weapon = null;

        private bool _isReloading = false;

        private Coroutine _reloadCoroutine;

        //EVENTS
        public event Action NoAmmunationAtInventory;
        public event Action Reloading;
        public event Action Reloaded;
        public event Action ReloadingCanceled;
        public event Action MagazineFull;

        //GETTERS AND SETTERS
        public int magazineSize => _weapon.magazieSize;

        public float reloadTime => _weapon.reloadTime;

        public int currentMagazine => _weapon.magazieSize;

        public bool isReloading => _isReloading;

        //Unity CALLBACKS

        //Interface Callback
        public void OnAwakeCalled(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void Reload()
        {
            if (_isReloading)
                return;
            if (_weapon.ammo <= 0)
            {
                NoAmmunationAtInventory?.Invoke();
                return;
            }
            if (_weapon.currentMagazine == magazineSize)
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

            var neededAmmo = magazineSize - _weapon.currentMagazine;
            if (neededAmmo >= _weapon.ammo)
            {
                _weapon.currentMagazine += _weapon.ammo;
                _weapon.ammo = 0;
            }
            else if (neededAmmo < _weapon.ammo)
            {
                _weapon.currentMagazine += neededAmmo;
                _weapon.ammo -= neededAmmo;
            }

            _isReloading = false;
            Reloaded?.Invoke();
        }

        public void DecreaseAmmo()
        {
            --_weapon.currentMagazine;
        }

        public bool ShootingAllowed()
        {
            return _weapon.currentMagazine > 0 && _isReloading == false;
        }
    }
}