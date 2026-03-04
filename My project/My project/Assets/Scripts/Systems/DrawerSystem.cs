namespace FpsHorrorKit
{
    using UnityEngine;

    public class DrawerSystem : MonoBehaviour, IInteractable
    {
        [Header("Interact Settings")]
        [SerializeField] private Sprite interactImage;

        [Header("Move Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float maxPosition = 0f;

        private float startPositionZ = 0f;

        private Collider _collider;
        private void Start()
        {
            _collider = GetComponent<Collider>();
            startPositionZ = transform.localPosition.z;
        }
        public void HoldInteract()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            // Fare hareketine göre rotasyon hesapla
            float rotationInput = Input.GetAxis("Mouse Y") * -moveSpeed * Time.deltaTime;
            float currentPos = transform.localPosition.z;
            float clampedPos = Mathf.Clamp(currentPos + rotationInput, startPositionZ, maxPosition);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, clampedPos);
        }


        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractImage(interactImage);
        }
        public void Interact() { }
        public void UnHighlight()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }
    }
}