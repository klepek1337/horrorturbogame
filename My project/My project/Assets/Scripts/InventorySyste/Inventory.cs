namespace FpsHorrorKit
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        [SerializeField] GameObject inventoryPanel;
        [SerializeField] private int inventorySize = 20;

        private Dictionary<Item, int> items = new Dictionary<Item, int>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                if (inventoryPanel.activeSelf)
                {
                    InteractCameraSettings.Instance?.ShowCursor();
                }
                else
                {
                    InteractCameraSettings.Instance?.HideCursor();
                }
            }
        }

        public bool AddItem(Item item, int quantity = 1)
        {
            if (!item.isStackable)
            {
                for (int i = 0; i < quantity; i++)
                {
                    if (items.Count >= inventorySize)
                    {
                        Debug.Log("Inventory is full!");
                        return false;
                    }
                    items[item] = 1; // Add as a single entry for non-stackable items
                }
                return true;
            }

            if (items.ContainsKey(item))
            {
                if (items[item] + quantity <= item.maxStackSize)
                {
                    items[item] += quantity;
                    return true;
                }
                else
                {
                    Debug.Log("Cannot stack beyond max stack size!");
                    return false;
                }
            }

            if (items.Count >= inventorySize)
            {
                Debug.Log("Inventory is full!");
                return false;
            }

            items[item] = quantity;
            return true;
        }

        public bool RemoveItem(Item item, int quantity = 1)
        {
            if (!items.ContainsKey(item))
            {
                Debug.Log("Item not in inventory!");
                return false;
            }

            if (items[item] <= quantity)
            {
                items.Remove(item);
                return true;
            }

            items[item] -= quantity;
            return true;
        }

        public bool IsInventoryFull()
        {
            return items.Count >= inventorySize;
        }

        public Dictionary<Item, int> GetItems()
        {
            return items;
        }
    }
}