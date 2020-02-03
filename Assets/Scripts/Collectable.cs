using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private InventoryItem _inventoryItem = null;

    [ContextMenu("Init Collectable")]
    private void Init()
    {
        _inventoryItem.InitGFXInScene(this);
    }

    public void SetInventoryItem(InventoryItem newItem)
    {
        if (_inventoryItem == null)
            _inventoryItem = newItem;
    }

    public override void Interact(Inventory _playerInventory)
    {
        bool isAdded = _playerInventory.AddItem(_inventoryItem);
        
        if (isAdded)
        {
            print(_playerInventory.name + "  picked up " + _inventoryItem.itemName);
            Destroy(gameObject);
        }
        else
        {
            print("Didn't add to inventory");
        }
    }
}