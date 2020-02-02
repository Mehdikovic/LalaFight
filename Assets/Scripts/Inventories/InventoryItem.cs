using UnityEngine;


public abstract class InventoryItem : ScriptableObject
{
    public string itemName = null;
    public Sprite icon = null;

    public virtual void InitInSceneGFX(Transform parent) { }
    
    public abstract void Use();
}