using UnityEngine;


namespace LalaFight
{
    public abstract class InventoryItem : ScriptableObject
    {
        public string itemName;
        public Sprite icon;

        public abstract void UseItem(Transform owner);
    }
}