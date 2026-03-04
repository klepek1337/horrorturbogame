using UnityEngine;

namespace FpsHorrorKit
{
    public class ITOLanternFuel : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item itemLanternFuel;
        [SerializeField] private string interactText = "Take lantern fuel [E]";

        public void Interact()
        {
            bool result = Inventory.Instance.AddItem(itemLanternFuel, 1);
            if (result)
            {
                InteractMessageScript.Instance?.ShowMessage("Lantern fuel taken! To use, open inventory(press I) and press the use button");
                UIInventory.Instance.UpdateUI();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}