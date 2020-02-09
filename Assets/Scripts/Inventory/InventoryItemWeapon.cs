using UnityEngine;


namespace LalaFight
{
    [CreateAssetMenu(menuName = "Inventory/Item/WeaponItem")]
    public class InventoryItemWeapon : InventoryItem
    {
        public Weapon prefab;
        public WeaponCollectable collectable;

        public override void UseItem(Transform owner)
        {
        }
    }
}