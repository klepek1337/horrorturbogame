namespace FpsHorrorKit
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerInteract : MonoBehaviour
    {
        public static PlayerInteract Instance { get; private set; }
        [Header("Raycast Settings")]
        public bool sendRaycast;
        public float interactRange = 2.0f; // Etkileşim mesafesi

        [Header("Highlight Settings")]
        public GameObject higlightObject;
        public TextMeshProUGUI interactTextUI;
        public Image interactImageUI;

        public bool showHiglight;

        private FpsAssetsInputs _input;
        private IInteractable currentInteractable;

        private GameObject defaultHighlightObj;
        private string defaultInteractText;
        [SerializeField] private bool canDragDoor;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _input = FindAnyObjectByType<FpsAssetsInputs>();

            showHiglight = true;
            sendRaycast = true;

            defaultInteractText = "Press [E] to interact";
            interactTextUI.text = defaultInteractText;

            defaultHighlightObj = higlightObject;
        }

        void Update()
        {
            if (currentInteractable != null)
            {
                if (Input.GetMouseButton(0) && canDragDoor)
                {
                    higlightObject.SetActive(false); // Highlight'i kaldırın
                    currentInteractable.HoldInteract();
                    sendRaycast = false;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    UnHighlight();
                    currentInteractable.UnHighlight();

                    canDragDoor = false;
                    sendRaycast = true;
                    currentInteractable = null;
                }
            }
            if (sendRaycast)
            {
                showHiglight = true;
                SendRaycast();
            }
            else
            {
                showHiglight = false;
            }
        }
        // Ray gönderme metodu
        private void SendRaycast()
        {
            // Kamera'nın merkezine bir ray yollayın
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit; // Etkilenen nesnenin bilgilerinide alın

            // Ray'ı etkilenen nesneyle karşılaştırın
            if (Physics.Raycast(ray, out hit, interactRange))
            {
                // Rayden etkilenen nesneyi işaretleyin
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // Rayden etkilenen nesne IInteractable interfase sahipse
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    canDragDoor = true;

                    Highlight(); // Highlight metodu çağırın

                    // Rayden etkilenen nesne ile etkileşime girmek istediğinizde E tusuna basın
                    if (_input.interact && higlightObject.activeSelf)
                    {
                        currentInteractable.Interact(); // Interact metodu çağırın
                        UnHighlight();
                        _input.interact = false;
                    }
                }
                // Rayden tkilenen nesne IInteractable interfase sahip değilse
                else
                {
                    UnHighlight();
                }
            }
            // Etkilenen nesne yoksa
            else
            {
                UnHighlight();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
        // Highlight metodu
        private void Highlight()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Highlight();
            }
            higlightObject.SetActive(showHiglight);
        }
        // UnHighlight metodu
        private void UnHighlight()
        {
            canDragDoor = false;

            higlightObject.SetActive(false);
            higlightObject = defaultHighlightObj;
            interactTextUI.text = defaultInteractText;
        }
        public void ChangeInteractText(string interactText)
        {
            interactTextUI.text = interactText;
            higlightObject = interactTextUI.gameObject;
        }
        public void ChangeInteractImage(Sprite interactImage)
        {
            interactImageUI.sprite = interactImage;
            higlightObject = interactImageUI.gameObject;
        }
    }
}