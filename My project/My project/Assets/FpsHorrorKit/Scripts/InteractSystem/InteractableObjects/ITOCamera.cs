namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITOCamera : MonoBehaviour, IInteractable
    {
        public Item photoCamera;
        [SerializeField] private string interactText = "Take camera [E]";

        public void Interact()
        {
            InteractMessageScript.Instance?.ShowMessage("Camera taken! To use, press 2 then T.");
            photoCamera.hasItem = true;
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
