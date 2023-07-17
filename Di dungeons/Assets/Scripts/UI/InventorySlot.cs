using UnityEngine;
using UnityEngine.UI;

namespace UB
{
    public class InventorySlot : MonoBehaviour
    {
        Item item;

        public Image icon;
        public Button removeButton;

        public void AddItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.ItemIcon;
            icon.enabled = true;
            removeButton.interactable = true;
        }

        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;

            removeButton.interactable = false;
        }

        public void OnRemoveButton()
        {
            Inventory.instance.Remove(item);
        }

        public void UseItem()
        {
            if(item != null)
            {
                item.Use();
            }
        }

        public void OnUnequipButton()
        {
            Equipment equipmentToUnequip = item as Equipment;
            ClearSlot();
            int slotIndex = (int)equipmentToUnequip.equipmentSlot;
            PlayerEquipmentManager.instance.Unequip(slotIndex);
            PlayerEquipmentManager.instance.EquipDefaultItems();

            //display default item in slot if there is one
            if(PlayerEquipmentManager.instance.CurrentEquipment[slotIndex] != null)
                AddItem(PlayerEquipmentManager.instance.CurrentEquipment[slotIndex]);
        }
    }
}

