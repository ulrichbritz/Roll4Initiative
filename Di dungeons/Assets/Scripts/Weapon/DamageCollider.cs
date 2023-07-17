using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public int currentWeaponDamage;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<CharacterController>() == GameManager.instance.activeCharacter.allTargets[GameManager.instance.activeCharacter.currentTarget])
            {
                CharacterStats characterStats = other.GetComponent<CharacterStats>();

                if(characterStats != null)
                {
                    characterStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}

