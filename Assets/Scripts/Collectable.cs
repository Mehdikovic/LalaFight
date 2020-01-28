using UnityEngine;

public class Collectable : Intractable
{
    [SerializeField] private InventoryItem _inventoryItem = null;

    public override void Use(PlayerController _player)
    {
        print(_player.name + "  picked up" + _inventoryItem.name);
        //TODO: add Item to player's Inventory
        Destroy(gameObject);
    }
}
