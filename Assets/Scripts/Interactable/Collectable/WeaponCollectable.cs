using UnityEngine;

public class WeaponCollectable : Interactable
{
    [SerializeField] private InventoryItemWeapon _weapon = null;
    [SerializeField] private int _rounds = 10;

    //GETTERS AND SETTERS
    public int rounds { get => _rounds; set => _rounds = value; }

    public override void Interact(Transform player)
    {
        var weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.Log("Player doesn't have WeaponManager");
            return;
        }

        var mountInfo = new WeaponMountInfo() { inventoryWeapon = _weapon, rounds = _rounds };
        var mountinfo = weaponManager.AddInventoryItem(mountInfo);

        if (mountinfo.inventoryWeapon != _weapon)
        {
            Destroy(gameObject);
            return;
        }

        if (mountinfo)
        {
            //TODO: instantiate collectable and destroy this object
            print(mountinfo.inventoryWeapon.itemName);
        }

        
    }
}
