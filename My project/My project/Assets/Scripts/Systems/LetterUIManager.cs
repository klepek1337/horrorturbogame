namespace FpsHorrorKit
{
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LetterUIManager : MonoBehaviour
    {
        public static LetterUIManager Instance { get; private set; }

        [Header("Letter UI")]
        [SerializeField] GameObject _letterUI;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _closeButton;

        [Header("Typing")]
        [SerializeField] private bool isTyping = false;
        [SerializeField] private float typingDelay = 0.1f;

        private bool isOpen = false;
        private FpsController _fpsController;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        void Start()
        {
            _fpsController = FindAnyObjectByType<FpsController>();

            _closeButton.onClick.AddListener(() => HideText());
        }

        public void ShowText(string text)
        {
            if (isOpen) return;

            if (isTyping)
            {
                StartCoroutine(Typing(text, typingDelay));
            }
            else
            {
                _text.text = text;
            }
            _letterUI.SetActive(true);
            isOpen = true;

            InteractCameraSettings.Instance?.Interacting(.1f);
            InteractCameraSettings.Instance?.ShowCursor();
            PlayerInteract.Instance.sendRaycast = false;
            _fpsController.isInteracting = true;
        }

        public void HideText()
        {
            StopAllCoroutines();

            _letterUI.SetActive(false);
            _text.text = "";
            isOpen = false;

            InteractCameraSettings.Instance?.NotInteracting();
            InteractCameraSettings.Instance?.HideCursor();
            PlayerInteract.Instance.sendRaycast = true;
            _fpsController.isInteracting = false;
        }

        IEnumerator Typing(string newText, float delay)
        {
            foreach (char letter in newText.ToCharArray())
            {
                _text.text += letter;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}