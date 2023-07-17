using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class EquipmentManager : MonoBehaviour
    {
        [Header("Scripts")]
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public delegate void OnEquipmentChanged(Equipment newItem = null, Equipment oldItem = null);
        public OnEquipmentChanged onEquipmentChanged;

        [SerializeField] Equipment[] defaultEquipment;
        [SerializeField] Equipment[] currentEquipment;
        public Equipment[] CurrentEquipment => currentEquipment;

        Inventory inventory;

        private void Awake()
        {
           //maybe make an instance
        }

        private void Start()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

            inventory = Inventory.instance;

            if(currentEquipment.Length <= 0)
            {
                int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
                currentEquipment = new Equipment[numSlots];
            }

            EquipDefaultItems();

            rightWeapon = currentEquipment[(int)EquipmentSlot.Weapon] as WeaponItem;
            leftWeapon = currentEquipment[(int)EquipmentSlot.OffHand] as WeaponItem;

            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);


        }

        public virtual void Equip(Equipment newItem)
        {
            int slotIndex = (int)newItem.equipmentSlot;

            Equipment oldItem = Unequip(slotIndex);

            if(newItem as WeaponItem != null)
            {
                WeaponItem newWeapon = newItem as WeaponItem;
                if(newWeapon.isTwoHanded == true)
                {
                    Unequip((int)EquipmentSlot.OffHand);
                }
            }

            if(onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(newItem, oldItem);
            }

            currentEquipment[slotIndex] = newItem;

            if((slotIndex == (int)EquipmentSlot.Weapon) || (slotIndex == (int)EquipmentSlot.OffHand))
            {
                rightWeapon = currentEquipment[(int)EquipmentSlot.Weapon] as WeaponItem;
                leftWeapon = currentEquipment[(int)EquipmentSlot.OffHand] as WeaponItem;

                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
        }

        public virtual Equipment Unequip(int slotIndex)
        {
            Equipment oldItemHolder = null;

            if(currentEquipment[slotIndex] != null)
            {
                Equipment oldItem = currentEquipment[slotIndex];
                inventory.Add(oldItem);

                currentEquipment[slotIndex] = null;

                if (onEquipmentChanged != null)
                {
                    onEquipmentChanged.Invoke(null, oldItem);
                }

                oldItemHolder = oldItem;
            }

            if ((slotIndex == (int)EquipmentSlot.Weapon) || (slotIndex == (int)EquipmentSlot.OffHand))
            {
                if (currentEquipment[(int)EquipmentSlot.Weapon] != null)
                    rightWeapon = currentEquipment[(int)EquipmentSlot.Weapon] as WeaponItem;
                if (currentEquipment[(int)EquipmentSlot.OffHand] != null)
                    leftWeapon = currentEquipment[(int)EquipmentSlot.OffHand] as WeaponItem;

                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }

            return oldItemHolder;
        }

        public void EquipDefaultItems()
        {
            foreach (Equipment equipment in defaultEquipment)
            {
                int slotIndex = (int)equipment.equipmentSlot;
                if (currentEquipment[slotIndex] == null)
                {
                    Equip(equipment);
                }
            }

           // inventory.onItemChangedCallback.Invoke();
        }
    }
}

