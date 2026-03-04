namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITOInfoMessage : MonoBehaviour, IInteractable
    {
        [Header("Interact Text")]
        [SerializeField] private string interactText = "Read Message [E]";

        [Header("Message")]
        [TextArea(3, 10)]
        [SerializeField] private string _message;

        public void Interact()
        {
            LetterUIManager.Instance.ShowText(_message);
        }
        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}