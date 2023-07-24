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
            CheckIfNeedMouse();
        }

        public void ToggleEquipment()
        {
            equipmentUIObject.SetActive(!equipmentUIObject.activeSelf);
            CheckIfNeedMouse();
        }

        private void CheckIfNeedMouse()
        {
            if(inventoryUIObject.activeSelf == true || equipmentUIObject == true)
            {
                MouseControls.instance.ShowCursor();
                PlayerManager.instance.uiFlag = true;
            }
            
            if(inventoryUIObject.activeSelf == false || equipmentUIObject == false)
            {
                MouseControls.instance.HideCursor();
                PlayerManager.instance.uiFlag = false;
            }
        }
    }
}

