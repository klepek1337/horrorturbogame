namespace FpsHorrorKit
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class HidingSystem : MonoBehaviour, IInteractable
    {
        [Header("Transform References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform outPoint;
        [SerializeField] private Transform hidingPoint;

        [Header("Door Settings")]
        [SerializeField] private AudioSource doorAudioSource;
        [SerializeField][Tooltip("Kapının dönme açısı")] private float endRotation;
        [SerializeField][Tooltip("Bu değişken kamera değişim hızı ile aynı olmalıdır")] private float hidingSpeed = 1f;
        [SerializeField][Tooltip("Kapının menteşe etrafında dönme hızı")] private float rotationSpeed = 100f;

        [Header("UI")]
        [SerializeField] private string interactText = "Hide/Come out [E]";

        private bool isInteract;
        private FpsController _fpsController;
        private Collider _collider;
        private float startRotation;

        private void Start()
        {
            startRotation = transform.localEulerAngles.y;
            _collider = GetComponent<Collider>();
            _fpsController = FindAnyObjectByType<FpsController>();
        }

        public void Interact()
        {
            StopAllCoroutines();
            if (!isInteract)
            {
                StartCoroutine(HandleInteraction(hidingSpeed, Hiding, hidingPoint));
            }
            else
            {
                StartCoroutine(HandleInteraction(hidingSpeed, ComeOut, outPoint));
            }
            isInteract = !isInteract;
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        private void ComeOut()
        {
            DisableCollider();
            _fpsController.isInteracting = false;
            StartCoroutine(MovePlayerSmoothly(outPoint, hidingSpeed));
            EnableCollider();
        }

        private void Hiding()
        {
            DisableCollider();
            _fpsController.isInteracting = true;
            StartCoroutine(MovePlayerSmoothly(hidingPoint, hidingSpeed));
        }

        private IEnumerator HandleInteraction(float delay, Action interactionAction, Transform targetPoint)
        {
            interactionAction();
            StartCoroutine(OpenDoor(startRotation, endRotation));

            yield return new WaitForSeconds(delay);
        }

        private IEnumerator MovePlayerSmoothly(Transform targetTransform, float duration)
        {
            Vector3 startPosition = playerTransform.position;
            Quaternion startRotation = playerTransform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                playerTransform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
                playerTransform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTransform.position = targetTransform.position;
            playerTransform.rotation = targetTransform.rotation;
        }

        private IEnumerator OpenDoor(float initialRotation, float targetRotation)
        {
            PlayDoorAudio();
            yield return RotateDoor(initialRotation, targetRotation);
            yield return RotateDoor(targetRotation, initialRotation);
            EnableCollider();
        }

        private void PlayDoorAudio()
        {
            if (doorAudioSource != null)
                doorAudioSource.Play();
        }

        private IEnumerator RotateDoor(float fromRotation, float toRotation)
        {
            while (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, toRotation)) > 0.1f)
            {
                float step = rotationSpeed * Time.deltaTime;
                float newY = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, toRotation, step);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                yield return null;
            }
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, toRotation, transform.localEulerAngles.z);
        }

        private void EnableCollider()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }

        private void DisableCollider()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}