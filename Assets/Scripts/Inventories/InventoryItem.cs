using UnityEngine;


public abstract class InventoryItem : ScriptableObject
{
    public string itemName = null;
    public Sprite icon = null;

    public abstract void Use();
}