namespace FpsHorrorKit
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        [Tooltip("'The name of the character.'")] public string characterName;
        [Tooltip("'If true, the dialogue will automatically advance.'")] public bool isAutoAdvance;
        [Tooltip("'If true, the text will be typed one letter at a time.'")] public bool typeTextEffect;
        [Tooltip("'If true, this script's auto-advance speed will be used.'")] public bool useThisAutoAdvanceSpeed;
        [Tooltip("'The speed at which the text will be typed.'")][Range(1, 50)] public float autoAdvanceSpeed;
        [Tooltip("'The lines of dialogue.'")][TextArea(3, 5)] public string[] dialogueLines;
        [Tooltip("'The audio clips for the dialogue.'")] public AudioClip[] audioClips;

    }

}
