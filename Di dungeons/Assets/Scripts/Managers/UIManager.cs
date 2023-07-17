using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [SerializeField] GameObject inventoryUIObject;
        [SerializeField] GameObject equipmentUIObject;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ToggleInventory()
        {
            inventoryUIObject.SetActive(!inventoryUIObject.activeSelf);
        }

        public void ToggleEquipment()
        {
            equipmentUIObject.SetActive(!equipmentUIObject.activeSelf);
        }
    }
}

