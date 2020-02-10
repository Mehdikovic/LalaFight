using UnityEngine;


namespace LalaFight
{
    public class HUDSpanwer : MonoBehaviour
    {
        [SerializeField] GameOverUI _gameOverUIPrefab = null;
        [SerializeField] WeaponReloadTimerUI _weaponReloadUIPrefab = null;
        [SerializeField] WeaponHUDUI _weaponHUDUIPrefab = null;

        //[SerializeField] GameObject _playerHUDPrefab = null;

        private WeaponManager _weaponManager;

        private WeaponReloadTimerUI _reloadUI;
        private WeaponHUDUI _weaponUI;

        private void Awake()
        {
            Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();

            _weaponManager = GetComponent<WeaponManager>();
            _weaponManager.WeaponHideStateChanged += WeaponManager_OnWeaponHideStateChanged;
            _weaponManager.WeaponAdded += WeaponManager_OnWeaponAdded;
            _weaponManager.WeaponSwapped += WeaponManager_OnWeaponSwapped;

            GetComponent<AmmoManager>().AmmoAdded += AmmoManager_OnAmmoAdded;

            if (mainCanvas == null)
                return;

            if (_gameOverUIPrefab != null)
            {
                var gameOverUI = Instantiate(_gameOverUIPrefab, mainCanvas.transform);
                gameOverUI.Init(player: transform);
            }

            if (_weaponReloadUIPrefab != null)
                _reloadUI = Instantiate(_weaponReloadUIPrefab, mainCanvas.transform);

            if (_weaponHUDUIPrefab != null)
                _weaponUI = Instantiate(_weaponHUDUIPrefab, mainCanvas.transform);

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