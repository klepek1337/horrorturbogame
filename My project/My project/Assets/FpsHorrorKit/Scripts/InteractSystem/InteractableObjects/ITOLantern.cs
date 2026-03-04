namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITOLantern : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item item;
        [SerializeField] private string interactText = "Take lantern [E]";
        [SerializeField] private ITOLightSwitch mainLightSwitch;

        public void Interact()
        {
            item.hasItem = true;
            if(mainLightSwitch != null)
                mainLightSwitch.Interact(); // Close the all lights
            InteractMessageScript.Instance?.ShowMessage("Lantern taken! To use, press 1 then F.");
            Destroy(gameObject);
        }
        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}