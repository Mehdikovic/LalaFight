using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/WeaponItem")]
public class WeaponInventoryItem : InventoryItem
{
    public GameObject prefab;

    public override void Use()
    {
        Debug.Log("[USED]: " + name);
    }
}
