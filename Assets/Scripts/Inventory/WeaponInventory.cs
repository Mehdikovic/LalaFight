using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponInfoMount
{
    public InventoryItemWeapon inventoryItem;
    public int rounds;
}


[CreateAssetMenu(menuName = "Inventory/WeaponInventory")]
public class WeaponInventory : ScriptableObject
{
    //EVENTS
    public Action<Weapon> WeaponAdded;
    public Action<Weapon> WeaponRemoved;
    
    
    public void AddItem(WeaponInfoMount weaponInfoMount)
    {

    }
}
