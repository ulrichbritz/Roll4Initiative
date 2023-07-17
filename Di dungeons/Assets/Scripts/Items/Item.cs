using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        [SerializeField] bool isDefaultItem = false;
        [HideInInspector] public bool IsDefaultItem => isDefaultItem;

        [SerializeField] Sprite itemIcon;
        [HideInInspector] public Sprite ItemIcon => itemIcon;

        [SerializeField] string itemName;
        [HideInInspector] public string ItemName => itemName;

        public virtual void Use()
        {
            Debug.Log("Using " + ItemName);
        }

        public void RemoveFromInventory()
        {
            Inventory.instance.Remove(this);
        }
    }
}

