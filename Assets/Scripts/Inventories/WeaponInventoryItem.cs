using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/WeaponItem")]
public class WeaponInventoryItem : InventoryItem
{
    public Weapon weaponPrefab;

    public override void InitInSceneGFX(Transform parent)
    {
        Debug.Log("Init GFX called");
    }

    public override void Use()
    {
        Debug.Log("[USED]: " + name);
    }
}
