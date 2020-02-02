using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/WeaponItem")]
public class WeaponInventoryItem : InventoryItem
{
    [Header("Weapon Prefab")]
    public Weapon weaponPrefab;

    public override void InitGFXInScene(Collectable parent)
    {
        Quaternion rotation = Quaternion.AngleAxis(-90, Vector3.right);
        
        Instantiate(weaponGFXInScene, parent.transform.position.WithY(.2f), rotation, parent.transform);
    }

    public override void Use(GameObject owner)
    {
        WeaponManager manager = owner.GetComponent<WeaponManager>();
        if (manager == null)
            return;
        
        var newWeapon = Instantiate(weaponPrefab, owner.transform);
        var oldWeapon = manager.AddWeapon(newWeapon);

        if (oldWeapon == null)
            return;
        
        Destroy(oldWeapon.gameObject);
    }
}
