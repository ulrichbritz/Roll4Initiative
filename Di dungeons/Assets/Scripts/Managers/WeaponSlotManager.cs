using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class WeaponSlotManager : MonoBehaviour
    {
        Animator anim;

        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot secondaryWeaponSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;
        
        private void Awake()
        {
            anim = GetComponent<Animator>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isSecondarySlot)
                {
                    secondaryWeaponSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                

                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.left_hand_idle, 0.2f);
                    //get collider if not null
                    LoadLeftHandDamageCollider();
                }
                else
                {
                    //if two handed equiped
                    if (rightHandSlot.currentWeapon != null && rightHandSlot.currentWeapon.isTwoHanded)
                        anim.CrossFade(rightHandSlot.currentWeapon.left_hand_idle, 0.2f);
                    else
                        anim.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                

                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    //get collider if not null
                    LoadRightHandDamageCollider();

                    //check if two handed
                    if (weaponItem.isTwoHanded)
                        leftHandSlot.UnloadWeaponAndDestroy();

                    anim.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    anim.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
        }

        #region Handle Weapon Damage Collider
        private void LoadLeftHandDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightHandDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void SetRightHandDamageColliderDamage(int damage)
        {
            rightHandDamageCollider.currentWeaponDamage = damage;
        }

        public void SetLeftHandDamageColliderDamage(int damage)
        {
            leftHandDamageCollider.currentWeaponDamage = damage;
        }

        public void OpenRightHandDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion

    }
}

