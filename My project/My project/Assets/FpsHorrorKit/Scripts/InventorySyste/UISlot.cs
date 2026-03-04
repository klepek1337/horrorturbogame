namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UISlot : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Button useButton;

        private void Awake()
        {
            useButton.onClick.AddListener(UseItem);
        }

        public void SetSlot(Item item, int quantity)
        {
            icon.sprite = item.icon;
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
            this.item = item;
        }
        public void UseItem()
        {
            item.useItemAction?.Invoke();
            Inventory.Instance.RemoveItem(item, 1);
            UIInventory.Instance.UpdateUI();
        }
    }
}