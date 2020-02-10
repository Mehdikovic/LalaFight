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
            _weaponManager.WeaponHideStateChanged += _weaponManager_WeaponHideStateChanged;
            _weaponManager.WeaponAdded += _weaponManager_WeaponAdded;
            _weaponManager.WeaponSwapped += _weaponManager_WeaponSwapped;

            GetComponent<AmmoManager>().AmmoAdded += HUDSpanwer_AmmoAdded;

            if (mainCanvas == null)
                return;

            if (_gameOverUIPrefab != null)
            {
                var obj = Instantiate(_gameOverUIPrefab, mainCanvas.transform);
                obj.Init(transform);
            }

            if (_weaponReloadUIPrefab != null)
                _reloadUI = Instantiate(_weaponReloadUIPrefab, mainCanvas.transform);

            if (_weaponHUDUIPrefab != null)
                _weaponUI = Instantiate(_weaponHUDUIPrefab, mainCanvas.transform);

        }

        private void HUDSpanwer_AmmoAdded()
        {
            if (_weaponUI != null)
                _weaponUI.Bind(_weaponManager.currentWeapon);

            _weaponUI.Activate(!_weaponManager.isWeaponHided);
        }

        private void _weaponManager_WeaponHideStateChanged(bool isHided)
        {
            if (_weaponUI != null)
                _weaponUI.Activate(!isHided);
        }

        private void _weaponManager_WeaponSwapped(Weapon oldWeapon, Weapon newWeapon)
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

        private void _weaponManager_WeaponAdded(Weapon newWeapon)
        {
            if (_reloadUI != null)
                _reloadUI.Bind(newWeapon);

            if (_weaponUI != null)
                _weaponUI.Bind(newWeapon);
        }
    }
}