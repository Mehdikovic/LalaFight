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

        WeaponInventory weaponInv = weaponManager.Inventory;
        //weaponInv.AddItem(new WeaponInfoMount { inventoryItem = _weapon, rounds = _rounds });
    }
}
