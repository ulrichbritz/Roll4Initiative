using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class InventoryUI : MonoBehaviour
    {
        public Transform itemsParent;

        Inventory inventory;

        InventorySlot[] slots;

        private void Start()
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateUI;

            slots = itemsParent.GetComponentsInChildren<InventorySlot>();

            UpdateUI();
        }


        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }
    }
}

