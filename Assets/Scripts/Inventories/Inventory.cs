using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<InventoryItem> _items;
    private int _capacity = 4;
    private GameObject _owner;

    private void Awake()
    {
        _items = new List<InventoryItem>(_capacity);
        _owner = gameObject.transform.parent.gameObject;
    }

    public bool AddItem(InventoryItem newItem)
    {
        if (_items.Count >= _capacity)
            return false;

        if (_items.Contains(newItem))
            return false;
        
        _items.Add(newItem);
        return true;
    }

    [ContextMenu("Use first Item")]
    public void UseItem()
    {
        
        if (_items.Count > 0)
        {
            _items[0].Use(_owner);
            _items.RemoveAt(0);
        }
    }

    [ContextMenu("Drop first Item")]
    public void DropItem()
    {
        if (_items.Count > 0)
        {
            _items.RemoveAt(0);
        }
    }

}
