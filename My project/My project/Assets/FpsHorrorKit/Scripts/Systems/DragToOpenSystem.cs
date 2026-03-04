namespace FpsHorrorKit
{
    using UnityEngine;

    public class DragToOpenSystem : MonoBehaviour, IInteractable
    {
        public enum DoorDirection { left, right, front, back }

        [Header("Rotation Settings")]
        [Tooltip("Mouse hareket hassasiyeti")]
        [SerializeField] private float rotationSpeed = 5f;

        [Tooltip("Kapının kapalı konumdaki açı değeri (örn. 0)")]
        [SerializeField] private float minAngle = 0f;

        [Tooltip("Kapının tamamen açıldığı açı (örn. 90)")]
        [SerializeField] private float maxAngle = 90f;

        [Tooltip("Kapı'nın player ile etkileşim durumunu kontrol eden yön")]
        [SerializeField] private DoorDirection doorDirection = DoorDirection.left;

        [Header("Collider Settings")]
        [SerializeField] private bool colliderDisabledDuringInteraction = false;

        [Header("Intercact Text")]
        [SerializeField] Sprite interactImageUi;

        private float currentAngle = 0f;
        private float initialAngle;
        private Vector3 initialForward;
        private Collider _collider;
        private Transform player;

        void Start()
        {
            _collider = GetComponent<Collider>();
            player = GameObject.FindGameObjectWithTag("Player").transform;

            initialAngle = transform.localEulerAngles.y;

            // Kapının başlangıçtaki sağ yönünü saklıyoruz.
            switch (doorDirection)
            {
                case DoorDirection.left:
                    initialForward = -transform.right;
                    break;
                case DoorDirection.right:
                    initialForward = transform.right;
                    break;
                case DoorDirection.front:
                    initialForward = transform.forward;
                    break;
                case DoorDirection.back:
                    initialForward = -transform.forward;
                    break;
            }
        }

        public void Interact()
        {
        }

        public void HoldInteract()
        {
            if (colliderDisabledDuringInteraction && _collider != null)
            {
                _collider.enabled = false;
            }
            // Mouse'un X eksenindeki hareketi alıyoruz.
            float mouseX = Input.GetAxis("Mouse X");

            // Player'ın kapıya göre solunda mı sağında mı olduğunu belirlemek için:
            float sideMultiplier = 1f; // varsayılan
            if (player != null)
            {
                // Kapı pivotundan player'a doğru vektör
                Vector3 doorToPlayer = player.position - transform.position;
                // initialRight vektörü ile dot product hesaplıyoruz.
                // Eğer sonuç pozitifse, player kapının sağındadır.
                float dot = Vector3.Dot(initialForward, doorToPlayer);

                // Burada; eğer player sağdaysa mouse hareketinin etkisini tersine çeviriyoruz.
                sideMultiplier = (dot > 0) ? -1f : 1f;
            }

            // Mouse hareketine göre açıyı güncelliyoruz.
            currentAngle += mouseX * rotationSpeed * sideMultiplier;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            float targetAngle = initialAngle + currentAngle;

            // Kapıyı Y ekseni etrafında döndürüyoruz.
            transform.localEulerAngles = new Vector3(0, targetAngle, 0);
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractImage(interactImageUi);
        }

        public void UnHighlight()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }
    }
}