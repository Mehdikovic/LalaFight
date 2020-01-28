using UnityEngine;

public class Collectable : Intractable
{
    [SerializeField] private InventoryItem _inventoryItem = null;
    [SerializeField] private GameObject _parent = null;

    public override void Use(Transform _player)
    {
        print(_player.name + "  picked up " + _inventoryItem.itemName);
        //TODO: add Item to player's Inventory
        Destroy(_parent);
    }
}
