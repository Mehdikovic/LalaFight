using System;
using UnityEngine;


namespace LalaFight
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] GameOverUI _gameOverUIPrefab = null;
        [SerializeField] WeaponReloadTimerUI _weaponReloadUIPrefab = null;
        [SerializeField] WeaponHUDUI _weaponHUDUIPrefab = null;
        [SerializeField] HealthHUDUI _healthHUDPrefab = null;


        private WeaponManager _weaponManager;
        private PlayerHealthController _playerHealthController;
        private PlayerController _playerController;

        private GameOverUI _gameOverUI;
        private WeaponReloadTimerUI _reloadUI;
        private WeaponHUDUI _weaponUI;
        private HealthHUDUI _healthUI;

        private void Awake()
        {
            Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();

            _weaponManager = GetComponent<WeaponManager>();
            _playerHealthController = GetComponent<PlayerHealthController>();
            _playerController = GetComponent<PlayerController>();


            _weaponManager.WeaponHideStateChanged += WeaponManager_OnWeaponHideStateChanged;
            _weaponManager.WeaponAdded += WeaponManager_OnWeaponAdded;
            _weaponManager.WeaponSwapped += WeaponManager_OnWeaponSwapped;

            _playerHealthController.DamageTaken += PlayerHealthController_DamageTaken;
            _playerHealthController.ArmorDamageTaken += PlayerHealthController_ArmorDamageTaken;
            
            _playerHealthController.OnDeath += OnPlayerDeath;
            
            _playerController.OnPlayerFall += OnPlayerDeath;

            GetComponent<AmmoManager>().AmmoAdded += AmmoManager_OnAmmoAdded;

            if (mainCanvas == null)
                return;

            if (_gameOverUIPrefab != null)
            {
                _gameOverUI = Instantiate(_gameOverUIPrefab, mainCanvas.transform);
                _gameOverUI.Init();
            }

            if (_weaponReloadUIPrefab != null)
                _reloadUI = Instantiate(_weaponReloadUIPrefab, mainCanvas.transform);

            if (_weaponHUDUIPrefab != null)
                _weaponUI = Instantiate(_weaponHUDUIPrefab, mainCanvas.transform);

            if (_healthHUDPrefab != null) { 
                _healthUI = Instantiate(_healthHUDPrefab, mainCanvas.transform);
                _healthUI.Init(_playerHealthController, null);
            }

        }

        private void OnPlayerDeath()
        {
            _gameOverUI.OnPlayerDeath();

            DeatctiveAllUIs();
        }

        private void DeatctiveAllUIs()
        {
            if (_healthUI != null)
                _healthUI.gameObject.SetActive(false);

            if (_reloadUI != null)
                _reloadUI.gameObject.SetActive(false);

            if (_weaponUI != null)
                _weaponUI.gameObject.SetActive(false);
        }

        private void PlayerHealthController_ArmorDamageTaken()
        {
            if (_healthUI != null)
                _healthUI.UpdateUI();
        }

        private void PlayerHealthController_DamageTaken()
        {
            if (_healthUI != null)
                _healthUI.UpdateUI();
        }

        private void AmmoManager_OnAmmoAdded()
        {
            if (_weaponUI != null)
                _weaponUI.Bind(_weaponManager.currentWeapon);

            if (_weaponManager.currentWeapon)
                _weaponUI.Activate(!_weaponManager.isWeaponHided);
        }

        private void WeaponManager_OnWeaponHideStateChanged(bool isHided)
        {
            if (_weaponUI != null)
                _weaponUI.Activate(!isHided);
        }

        private void WeaponManager_OnWeaponSwapped(Weapon oldWeapon, Weapon newWeapon)
        {
            if (_reloadUI != null)
            {
                _reloadUI.UnBind();
                _reloadUI.Bind(newWeapon);
            }

            if (_weaponUI != null)
            {
                _weaponUI.UnBind();
                _weaponUI.Bind(newWeapon);
            }
        }

        private void WeaponManager_OnWeaponAdded(Weapon newWeapon)
        {
            if (_reloadUI != null)
                _reloadUI.Bind(newWeapon);

            if (_weaponUI != null)
                _weaponUI.Bind(newWeapon);
        }
    }
}