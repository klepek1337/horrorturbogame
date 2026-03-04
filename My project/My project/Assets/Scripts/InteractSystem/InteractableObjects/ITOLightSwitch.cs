namespace FpsHorrorKit
{
    using System.Collections;
    using UnityEngine;

    public class ITOLightSwitch : MonoBehaviour, IInteractable
    {
        [Header("Light Settings")]
        public GameObject _light;
        public Transform lightSwitchButton;
        public AudioSource switcherAudioSource;
        public bool lightActiveOnAwake = false;

        [Header("Rotation Settings")]
        public float onRotationAngle;
        public float offRotationAngle;
        public float rotationSpeed = 200f;

        [Header("Interact UI")]
        [SerializeField] private string interactText = "Light Open/Close [E]";

        bool isFinished = true;
        private float initialRotationX;

        private void Awake()
        {
            if (lightActiveOnAwake)
            {
                _light.SetActive(true);
                lightSwitchButton.localEulerAngles = new Vector3(onRotationAngle, 0, 0);
            }
            else
            {
                _light.SetActive(false);
                lightSwitchButton.localEulerAngles = new Vector3(offRotationAngle, 0, 0);
            }
        }
        private void Start()
        {
            isFinished = true;
            // Store the initial local rotation
            initialRotationX = lightSwitchButton.localEulerAngles.x;
        }

        IEnumerator RotateSwitcher(float targetRotation, float initialRotation)
        {
            isFinished = false;
            if (switcherAudioSource != null) switcherAudioSource.Play();

            float distance = Mathf.Abs(targetRotation - initialRotation);
            float currentRotation = initialRotation;
            float step = 0;

            int multiple = targetRotation > initialRotation ? 1 : -1;

            while (distance > 0.1f)
            {
                step = rotationSpeed * Time.deltaTime;
                distance -= step;

                currentRotation += step * multiple;
                lightSwitchButton.localEulerAngles = new Vector3(currentRotation, 0, 0);
                yield return null;
            }
            lightSwitchButton.localEulerAngles = new Vector3(targetRotation, 0, 0);
            // Finalize the rotation
            isFinished = true;
            Debug.Log("Light Switcher Rotated");
        }

        public void Interact()
        {
            if (!isFinished) return;

            _light.SetActive(!_light.activeSelf);
            float targetRotation = _light.activeSelf ? onRotationAngle : offRotationAngle;
            float initialRotation = _light.activeSelf ? offRotationAngle : onRotationAngle;

            StartCoroutine(RotateSwitcher(targetRotation, initialRotation));
        }


        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}