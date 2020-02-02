using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private InventoryItem _inventoryItem = null;

    //[ContextMenu("Init-Collectable")]
    private void OnEnable()
    {
        _inventoryItem.InitGFXInScene(this);
    }

    public override void Interact(Inventory _playerInventory)
    {
        print(_playerInventory.name + "  picked up " + _inventoryItem.itemName);
        bool isAdded = _playerInventory.AddItem(_inventoryItem);
        if (isAdded)
            Destroy(gameObject);
    }
}