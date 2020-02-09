using UnityEngine;


namespace LalaFight
{
    [CreateAssetMenu(menuName = "Inventory/Item/Ammo")]
    public class InventoryItemAmmo : InventoryItem
    {
        public BulletType bulletType;
        
        public override void UseItem(Transform owner)
        {
        }
    }
}