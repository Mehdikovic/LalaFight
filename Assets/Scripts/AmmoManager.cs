using System.Collections.Generic;
using UnityEngine;


namespace LalaFight
{
    [System.Serializable]
    public struct AmmoMountInfo
    {
        public InventoryItemAmmo inventoryItem;
        public int rounds;
    }

    public class AmmoManager : MonoBehaviour
    {
        [SerializeField] private AmmoMountInfo _smallAmmo = new AmmoMountInfo();
        [SerializeField] private AmmoMountInfo _mediumAmmo = new AmmoMountInfo();
        [SerializeField] private AmmoMountInfo _heavyAmmo = new AmmoMountInfo();

        private Dictionary<BulletType, AmmoMountInfo> _ammoInventory = null;

        public Dictionary<BulletType, AmmoMountInfo> ammoInventory => _ammoInventory;

        private void Awake()
        {
            _ammoInventory = new Dictionary<BulletType, AmmoMountInfo>();
            _ammoInventory.Add(BulletType.Small, _smallAmmo);
            _ammoInventory.Add(BulletType.Medium, _mediumAmmo);
            _ammoInventory.Add(BulletType.Heavy, _heavyAmmo);
        }

        public void AddAmmoToInventory(BulletType ammoType, int amount)
        {
            var mountInfo = _ammoInventory[ammoType];
            
            mountInfo.rounds = Mathf.Clamp(mountInfo.rounds + amount, 0, 999);
            
            _ammoInventory[ammoType] = mountInfo;
        }

        public int GetAmmo(BulletType ammoType) => _ammoInventory[ammoType].rounds;
        public void SetAmmo(BulletType ammoType, int value)
        {
            var mountInfo = _ammoInventory[ammoType];

            mountInfo.rounds = Mathf.Clamp(value, 0, 999);

            _ammoInventory[ammoType] = mountInfo;
        }
    }
}