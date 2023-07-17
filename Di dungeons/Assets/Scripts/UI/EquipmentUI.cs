using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class EquipmentUI : MonoBehaviour
    {
        public Transform itemsParent;

        EquipmentManager playerEquipmentManager;
        Inventory inventory;

        InventorySlot[] slots;

        private void Start()
        {
            inventory = Inventory.instance;
            playerEquipmentManager = PlayerEquipmentManager.instance;
            inventory.onItemChangedCallback += UpdateUI;

            slots = itemsParent.GetComponentsInChildren<InventorySlot>();

            UpdateUI();
        }


        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < playerEquipmentManager.CurrentEquipment.Length -1)
                {
                    if(playerEquipmentManager.CurrentEquipment[i] != null)
                        slots[i].AddItem(playerEquipmentManager.CurrentEquipment[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }
    }
}

