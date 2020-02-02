using UnityEngine;


public abstract class InventoryItem : ScriptableObject
{
    public string itemName = null;
    public Sprite icon = null;

    public virtual void InitGFXInScene(Collectable parent) { }
    
    public abstract void Use(GameObject owner);
}