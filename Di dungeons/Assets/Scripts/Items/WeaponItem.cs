using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Items/Equipment/Weapon Item")]
    public class WeaponItem : Equipment
    {
        [Header("Model")]
        public GameObject modelPrefab;
        public bool isUnarmed = false;
        public bool isTwoHanded = false;

        [Header("Damage")]
        public List<GameObject> weaponDiceList;

        [Header("isMelee")]
        public bool isMelee = true;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        [Header("Attack Animations")]
        public string light_attack_01;
        public string heavy_attack_01;

    }
}

