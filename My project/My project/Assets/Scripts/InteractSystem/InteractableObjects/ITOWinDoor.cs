namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ITOWinDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] private string winSceneName = "WinScene";
        [SerializeField] private string interactText = "Escape [E]";

        private bool isAlreadyTriggered = false;

        public void Interact()
        {
            if (isAlreadyTriggered)
            {
                return;
            }

            isAlreadyTriggered = true;
            SceneManager.LoadScene(winSceneName);
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract()
        {
        }

        public void UnHighlight()
        {
        }
    }
}