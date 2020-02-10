using System;
using UnityEngine;
using UnityEngine.UI;


namespace LalaFight
{
    public class WeaponHUDUI : MonoBehaviour
    {
        [Header("Holder")]
        [SerializeField] private GameObject _holder = null;

        [Header("Weapon Properties Holder")]
        [SerializeField] private Image _spriteUI = null;
        [SerializeField] private Text _currentMagazine = null;
        [SerializeField] private Text _inventoryAmmo = null;

        [SerializeField] private Text _fireMode = null;

        private Weapon _weapon = null;

        private void Awake()
        {
            ClearUI();
        }

        public void Bind(Weapon weapon)
        {
            if (weapon == null)
                return;
            
            _weapon = weapon;
            RegisterEvents();

            _holder.SetActive(true);
            _spriteUI.sprite = _weapon.sprite;
            
            if (_spriteUI.sprite == null)
                _spriteUI.color = Color.clear;
            else
                _spriteUI.color = Color.white;
            
            _currentMagazine.text = _weapon.currentMagazine.ToString();
            _inventoryAmmo.text = _weapon.ammo.ToString();
            _fireMode.text = weapon.GetCurrentFireMode().ToString();
        }

        public void UnBind()
        {
            ClearEvents();
            ClearUI();
        }

        private void ClearUI()
        {
            _holder.SetActive(false);
            _spriteUI.sprite = null;
            _spriteUI.color = Color.clear;
            _currentMagazine.text = "";
            _inventoryAmmo.text = "";
            _fireMode.text = "";
        }

        private void RegisterEvents()
        {
            _weapon.OnFireEnd += OnFireEnd;
            _weapon.magazine.Reloaded += Magazine_Reloaded;
            _weapon.FireModeChanged += Weapon_FireModeChanged;
        }

        private void ClearEvents()
        {
            _weapon.OnFireEnd -= OnFireEnd;
            _weapon.magazine.Reloaded -= Magazine_Reloaded;
            _weapon.FireModeChanged -= Weapon_FireModeChanged;
        }

        private void Weapon_FireModeChanged(FireModeType fireMode)
        {
            _fireMode.text = fireMode.ToString();
        }

        private void Magazine_Reloaded()
        {
            UpdateAmmoInfo();
        }

        public void Activate(bool activate)
        {
            _holder.SetActive(activate);
        }

        private void OnFireEnd()
        {
            UpdateAmmoInfo();
        }

        private void UpdateAmmoInfo()
        {
            _currentMagazine.text = _weapon.currentMagazine.ToString();
            _inventoryAmmo.text = _weapon.ammo.ToString();
        }
    }
}