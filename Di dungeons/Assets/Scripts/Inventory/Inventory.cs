using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;

        EquipmentManager equipmentManager;

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

        public int inventorySpace = 30;

        public List<Item> items = new List<Item>();

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

        private void Start()
        {
            equipmentManager = GetComponent<EquipmentManager>();
        }

        public bool Add(Item item)
        {
            if (!item.IsDefaultItem)
            {
                if(items.Count >= inventorySpace)
                {
                    Debug.Log("Not enough space in inventory");
                    return false;
                }

                items.Add(item);

                if(onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }

            return true;         
        }

        public void Remove(Item item)
        {
            items.Remove(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }
}

