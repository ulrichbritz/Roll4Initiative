using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Items/Equipment Item")]
    public class Equipment : Item
    {
        public EquipmentSlot equipmentSlot;

        public int armorModifier;

        public override void Use()
        {
            base.Use();

            //equip
            PlayerEquipmentManager.instance.Equip(this);

            //remove from inventory
            RemoveFromInventory();
        }
    }

    public enum EquipmentSlot { Head, Necklace, Chest, Hands, Ring, Weapon, OffHand, Legs, Feet}
}

