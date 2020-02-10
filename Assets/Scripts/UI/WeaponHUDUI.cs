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

        private Weapon _weapon = null;

        private void Awake()
        {
            ClearUI();
            _holder.SetActive(false);
        }

        public void Bind(Weapon weapon)
        {
            if (weapon == null)
                return;
            
            _weapon = weapon;
            RegisterEvents();

            _spriteUI.sprite = _weapon.sprite;
            
            if (_spriteUI.sprite == null)
                _spriteUI.color = Color.clear;
            else
                _spriteUI.color = Color.white;
            
            _currentMagazine.text = _weapon.currentMagazine.ToString();
            _inventoryAmmo.text = _weapon.ammo.ToString();
        }

        public void UnBind()
        {
            ClearEvents();
            ClearUI();
        }

        private void ClearUI()
        {
            _spriteUI.sprite = null;
            _spriteUI.color = Color.clear;
            _currentMagazine.text = "";
            _inventoryAmmo.text = "";
        }

        private void RegisterEvents()
        {
            _weapon.OnFireEnd += OnFireEnd;
            _weapon.magazine.Reloaded += Magazine_Reloaded;
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

        private void ClearEvents()
        {
            _weapon.OnFireEnd += OnFireEnd;
        }
    }
}
