namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITONpc : MonoBehaviour, IInteractable
    {
        [Tooltip("Dialogue datas for the NPC")] public DialogueData[] dialogueData;
        [SerializeField] private string interactText = "Talk Npc [E]";
        private int currentDialogueIndex = 0;

        private void Start()
        {
            currentDialogueIndex = 0;
        }
        public void Interact() // Interact with the NPC
        {
            bool dialogueStarted = DialogueSystem.Instance.StartDialogue(currentDialogueIndex, dialogueData, OpenHiglight); // Start the dialogue
            if (dialogueStarted)
            {
                PlayerInteract.Instance.showHiglight = false;
                currentDialogueIndex++;
            }
        }
        public void Highlight() // Highlight the NPC
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }
        private void OpenHiglight()
        {
            PlayerInteract.Instance.showHiglight = true;
        }
        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}