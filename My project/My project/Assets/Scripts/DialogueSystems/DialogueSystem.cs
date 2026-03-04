namespace FpsHorrorKit
{
    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; private set; }

        [Header("References")]
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;
        public GameObject dialoguePanel;
        public CanvasGroup skipInfoImage;

        [Header("Subtitle Speed Settings")]
        [Range(1, 50)]
        [Tooltip("Bir sonraki altyazıya geçme hızı")] public float AutoAdvanceSpeed = 12f;

        [Tooltip("Delay between each character")] public float typingSpeed = 0.05f;

        [HideInInspector]
        public DialogueData[] dialogues;

        [HideInInspector]
        public bool isDialogueFinished = false;

        private int currentDialogueIndex;
        private int currentLineIndex;
        private Coroutine autoAdvanceCoroutine;
        private Coroutine typingCoroutine;
        private float currentAutoAdvanceSpeed;
        private AudioSource audioSource;


        public Action onDialogueEnd;

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
            audioSource = GetComponent<AudioSource>();
            dialoguePanel.SetActive(false);
            isDialogueFinished = true;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SkipDialogue();
            }
        }

        public bool StartDialogue(int dialogueIndex, DialogueData[] dialogueDatas, Action onEndAction = null)
        {
            if (!isDialogueFinished) { Debug.Log("Dialogue is not finished."); return false; }

            if (dialogueIndex < 0 || dialogueIndex >= dialogueDatas.Length)
            {
                Debug.Log("Invalid dialogue index.");
                return false;
            }

            dialogues = dialogueDatas;
            isDialogueFinished = false;
            dialoguePanel.SetActive(true);
            currentDialogueIndex = dialogueIndex;
            currentLineIndex = 0;
            onDialogueEnd = onEndAction;
            DisplayCurrentLine();
            StartCoroutine(HideSkipImage(.5f));

            if (dialogues[currentDialogueIndex].isAutoAdvance)
            {
                if (autoAdvanceCoroutine != null)
                {
                    StopCoroutine(autoAdvanceCoroutine);
                }

                // Farklı dialogue'lerde auto advance hızı farklı olabilir. Bunu kontrol et ve ona göre hızı uygula.
                if (dialogues[currentDialogueIndex].useThisAutoAdvanceSpeed)
                {
                    currentAutoAdvanceSpeed = dialogues[currentDialogueIndex].autoAdvanceSpeed;
                }
                else
                {
                    currentAutoAdvanceSpeed = AutoAdvanceSpeed;
                }
                autoAdvanceCoroutine = StartCoroutine(AutoAdvanceDialogue());
            }
            return true;
        }

        private IEnumerator AutoAdvanceDialogue()
        {
            while (dialoguePanel.activeSelf)
            {
                if (currentLineIndex >= dialogues[currentDialogueIndex].dialogueLines.Length)
                {
                    EndDialogue();
                    yield break;
                }

                int lineLength = dialogues[currentDialogueIndex].dialogueLines[currentLineIndex].Length;
                float delay = Mathf.Max(lineLength / currentAutoAdvanceSpeed, 2.5f); // Ensure a minimum delay

                yield return new WaitForSeconds(delay);
                AdvanceDialogue();
            }
        }

        private void AdvanceDialogue()
        {
            if (!dialoguePanel.activeSelf) return;

            currentLineIndex++;
            if (currentLineIndex >= dialogues[currentDialogueIndex].dialogueLines.Length)
            {
                EndDialogue();
            }
            else
            {
                DisplayCurrentLine();
            }
        }

        private void DisplayCurrentLine()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            DialogueData currentDialogue = dialogues[currentDialogueIndex];
            characterNameText.text = currentDialogue.characterName;

            if (currentDialogue.audioClips != null)
            {
                if (currentLineIndex >= 0 && currentLineIndex < currentDialogue.audioClips.Length)
                {
                    audioSource.PlayOneShot(currentDialogue.audioClips[currentLineIndex]);
                }
            }

            if (currentDialogue.typeTextEffect)
            {
                typingCoroutine = StartCoroutine(TypeTextEffect(currentDialogue.dialogueLines[currentLineIndex]));
            }
            else
            {
                dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
            }
        }

        private IEnumerator TypeTextEffect(string line)
        {
            dialogueText.text = "";
            foreach (char letter in line)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        private void EndDialogue()
        {
            if (autoAdvanceCoroutine != null)
            {
                StopCoroutine(autoAdvanceCoroutine);
                autoAdvanceCoroutine = null;
            }

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialoguePanel.SetActive(false);
            Debug.Log("Dialogue ended.");
            isDialogueFinished = true;

            onDialogueEnd?.Invoke();
        }

        IEnumerator HideSkipImage(float speed)
        {
            skipInfoImage.alpha = 1;
            skipInfoImage.gameObject.SetActive(true);

            while (skipInfoImage.alpha > 0)
            {
                skipInfoImage.alpha -= Time.deltaTime * speed;
                yield return null;
            }
            skipInfoImage.gameObject.SetActive(false);
        }
        public void SkipDialogue()
        {
            if (autoAdvanceCoroutine != null)
            {
                StopCoroutine(autoAdvanceCoroutine);
                autoAdvanceCoroutine = null;
            }
            AdvanceDialogue();
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceDialogue());
        }
    }
}