namespace FpsHorrorKit
{
    using UnityEngine;

    public class UIInventory : MonoBehaviour
    {
        public static UIInventory Instance { get; private set; }

        [SerializeField] private Transform inventoryPanel;
        [SerializeField] private GameObject slotPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Inventory.Instance.GetItems();
            UpdateUI();
        }

        public void UpdateUI()
        {
            foreach (Transform child in inventoryPanel)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in Inventory.Instance.GetItems())
            {
                GameObject slot = Instantiate(slotPrefab, inventoryPanel);
                UISlot uiSlot = slot.GetComponent<UISlot>();
                uiSlot.SetSlot(item.Key, item.Value);
            }
        }
    }
}