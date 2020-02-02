using UnityEngine;


public abstract class InventoryItem : ScriptableObject
{
    [Header("Inventory Item")]
    public string itemName = null;
    public Sprite icon = null;
    public GameObject weaponGFXInScene;

    public virtual void InitGFXInScene(Collectable parent) { }
    
    public abstract void Use(GameObject owner);
}