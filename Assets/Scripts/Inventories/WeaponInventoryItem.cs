using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/WeaponItem")]
public class WeaponInventoryItem : InventoryItem
{
    public Weapon weaponPrefab;
    public GameObject weaponGFXInScene;

    public override void InitGFXInScene(Collectable parent)
    {
        Instantiate(weaponGFXInScene, parent.transform.position, Quaternion.identity, parent.transform);
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
