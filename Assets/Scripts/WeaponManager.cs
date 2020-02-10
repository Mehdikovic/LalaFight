using System;
using System.Collections.Generic;
using UnityEngine;


namespace LalaFight
{
    [Serializable]
    public struct WeaponMountInfo
    {
        public InventoryItemWeapon inventoryWeapon;
        public int rounds;

        static public implicit operator bool(WeaponMountInfo instance)
        {
            return instance.inventoryWeapon != null;
        }
    }

    public class WeaponManager : MonoBehaviour
    {
        [Range(1, 4)]
        [SerializeField] private int _allowedWeaponNumber = 3;
        [SerializeField] private Transform _weaponHolder = null;

        [Header("Starting Guns")]
        [SerializeField] private WeaponMountInfo _pistolStartGun = new WeaponMountInfo();
        [SerializeField] private WeaponMountInfo[] _otherStartGun = null;


        private Weapon[] _weapons;
        private InventoryItemWeapon[] _weaponInventoryItems;
        private HashSet<InventoryItemWeapon> _inventoryToWeapon;
        private int _currentWeaponIndex = 0;
        private bool _isWeaponHided = false;


        public event Action<Weapon, Weapon> WeaponSwapped;
        public event Action<Weapon> WeaponAdded;
        public event Action<bool> WeaponHideStateChanged;

        public Weapon currentWeapon => _weapons?[_currentWeaponIndex];
        public bool isWeaponHided => _isWeaponHided;
        public bool isWeaponAnimating => currentWeapon.isAnimating;

        private void Awake()
        {
            _weapons = new Weapon[_allowedWeaponNumber];
            _weaponInventoryItems = new InventoryItemWeapon[_allowedWeaponNumber];

            _inventoryToWeapon = new HashSet<InventoryItemWeapon>();

            var pistol = InstantiateWeapon(_pistolStartGun);
            pistol?.FastLoad(_isWeaponHided);

            if (_otherStartGun != null && _otherStartGun.Length > 0)
                for (int i = 0; i < _otherStartGun.Length; i++)
                    InstantiateWeapon(_otherStartGun[i]);

            _currentWeaponIndex = 0;
        }

        private Weapon InstantiateWeapon(WeaponMountInfo mountInfo)
        {
            if (!mountInfo)
                return null;
            if (_inventoryToWeapon.Contains(mountInfo.inventoryWeapon))
                return null;

            var weapon = Instantiate(mountInfo.inventoryWeapon.prefab, _weaponHolder.position, _weaponHolder.rotation, _weaponHolder);

            DoAddingWeaponToCollections(mountInfo, weapon, _currentWeaponIndex);
            ++_currentWeaponIndex;
            weapon.FastUnload();

            return weapon;
        }

        public void SetCursorPositionAndLookAt(Vector3 hitPoint, float length)
        {
            if (length > 1)
                _weaponHolder.LookAt(hitPoint);
        }

        public void SwapWeapon(int changeIndex)
        {
            if (currentWeapon == null || currentWeapon.isAnimating) 
                return;

            int weaponsCount = 0;
            for (int index = 0; index < _weapons.Length; ++index)
                if (_weapons[index] != null)
                    ++weaponsCount;

            if (weaponsCount > 1 && _isWeaponHided == false)
            {
                var oldCurrentWeaponIndex = _currentWeaponIndex;
                

                for (int i = 0; i < _weapons.Length; i++)
                {
                    _currentWeaponIndex += changeIndex;
                    _currentWeaponIndex = _currentWeaponIndex == _weapons.Length ? 0 : _currentWeaponIndex;
                    _currentWeaponIndex = _currentWeaponIndex == -1 ? _weapons.Length - 1 : _currentWeaponIndex;
                    if (currentWeapon)
                        break;
                }

                if (oldCurrentWeaponIndex == _currentWeaponIndex)
                    return;

                _weapons[oldCurrentWeaponIndex].WeaponUnload();
                _weapons[_currentWeaponIndex].WeaponLoad();
                WeaponSwapped?.Invoke(_weapons[oldCurrentWeaponIndex], _weapons[_currentWeaponIndex]);
            }
        }

        public float GetWeaponActiveRange() => currentWeapon == null ? 10 : currentWeapon.scopeRange;

        public void ToggleWeaponEnable()
        {
            if (_isWeaponHided == false)
                HideWeapon();
            else
                ShowWeapon();
        }

        public void ShowWeapon(bool fastLoad = false)
        {
            if (currentWeapon == null)
                return;

            if (_isWeaponHided == false || currentWeapon.isAnimating == true)
                return;

            _isWeaponHided = false;

            if (fastLoad == true)
                currentWeapon.FastLoad(_isWeaponHided);
            else
                currentWeapon.WeaponLoad();

            WeaponHideStateChanged?.Invoke(_isWeaponHided);
        }

        public void HideWeapon(bool fastUnload = false)
        {
            if (currentWeapon == null)
                return;

            if (_isWeaponHided == true || currentWeapon.isAnimating == true)
                return;

            _isWeaponHided = true;

            if (fastUnload == true)
                currentWeapon.FastUnload();
            else
                currentWeapon.WeaponUnload();

            WeaponHideStateChanged?.Invoke(_isWeaponHided);
        }

        public void CurrentWeaponTick()
        {
            if (currentWeapon == null)
                return;

            if (_isWeaponHided == true)
                return;

            currentWeapon.HandleUpdateInputs();
        }

        public WeaponMountInfo AddInventoryItem(WeaponMountInfo mountInfo)
        {
            if (!mountInfo)
                return mountInfo;

            if (_inventoryToWeapon.Contains(mountInfo.inventoryWeapon))
            {
                //TODO: add ammo to ammo inventory
                return new WeaponMountInfo();
            }

            Weapon newWeaponPrefab = mountInfo.inventoryWeapon.prefab;

            if (newWeaponPrefab.type == WeaponType.Pistol)
            {
                var oldWeapon = currentWeapon;
                oldWeapon?.FastUnload();
                var oldMountInfo = DoAddingWeaponWithSwapping(mountInfo, 0);

                _currentWeaponIndex = 0;
                _weapons[0].WeaponLoad();

                return oldMountInfo;
            }

            int index = 1;
            for (int i = 1; i < _weapons.Length; ++i)
                if (_weapons[i] != null)
                    ++index;

            if (index >= _allowedWeaponNumber)
            {
                if (_currentWeaponIndex == 0)
                    return mountInfo;

                var oldMountInfo = DoAddingWeaponWithSwapping(mountInfo, _currentWeaponIndex);
                _weapons[_currentWeaponIndex].WeaponLoad();
                return oldMountInfo;
            }

            currentWeapon?.FastUnload();
            DoAddingWeaponWithSwapping(mountInfo, index);
            _weapons[index].WeaponLoad();
            _currentWeaponIndex = index;
            return new WeaponMountInfo();
        }

        private WeaponMountInfo DoAddingWeaponWithSwapping(WeaponMountInfo mountInfo, int index)
        {
            var newWeapon = Instantiate(mountInfo.inventoryWeapon.prefab);
            SyncWeaponWithHolder(newWeapon);
            newWeapon.FastUnload();

            var oldWeapon = _weapons[index];
            var oldInventoryItem = _weaponInventoryItems[index];
            var oldWeaponMount = new WeaponMountInfo() { inventoryWeapon = oldInventoryItem, rounds = oldWeapon == null ? 0 : oldWeapon.currentMagazine };

            oldWeapon?.FastUnload();

            DoAddingWeaponToCollections(mountInfo, newWeapon, index);

            if (oldWeapon)
                Destroy(oldWeapon.gameObject);

            if (_inventoryToWeapon.Contains(oldInventoryItem))
                _inventoryToWeapon.Remove(oldInventoryItem);

            return oldWeaponMount;
        }

        private void DoAddingWeaponToCollections(WeaponMountInfo mountInfo, Weapon newWeapon, int index)
        {
            _weapons[index] = newWeapon;
            _weaponInventoryItems[index] = mountInfo.inventoryWeapon;
            _inventoryToWeapon.Add(mountInfo.inventoryWeapon);

            newWeapon.currentMagazine = mountInfo.rounds;
            newWeapon.SetOwner(transform);

            WeaponAdded?.Invoke(newWeapon);
        }

        private void SyncWeaponWithHolder(Weapon newWeapon)
        {
            newWeapon.transform.position = _weaponHolder.position;
            newWeapon.transform.rotation = _weaponHolder.rotation;
            newWeapon.transform.parent = _weaponHolder;
        }
    }
}