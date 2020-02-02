using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<InventoryItem> _items;
    private int _capacity = 4;

    private void Awake()
    {
        _items = new List<InventoryItem>(_capacity);
    }

    public void AddItem(InventoryItem newItem)
    {
        if (_items.Count < _capacity)
            _items.Add(newItem);
    }

    [ContextMenu("Use first Item")]
    public void UseItem()
    {
        if (_items.Count > 0)
        {
            _items[0].Use(gameObject.transform.parent.gameObject);
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
