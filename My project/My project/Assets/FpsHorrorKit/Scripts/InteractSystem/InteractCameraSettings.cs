namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.HighDefinition;

    public class InteractCameraSettings : MonoBehaviour
    {
        private Volume volume;
        private DepthOfField depthOfField;
        private float startFocusDistance;

        public static InteractCameraSettings Instance;
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

            // Sahnedeki Volume bileşenini al
            volume = FindAnyObjectByType<Volume>();

            if (volume != null && volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                startFocusDistance = depthOfField.focusDistance.value;
            }
            else
            {
                Debug.LogError("Depth of Field veya Volume bulunamadı!");
            }
        }
        public void Interacting(float focusDistanceWhenInspecting = .5f) // Depth of Field'ı etkinleştir veya devre dışı bırak
        {
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = focusDistanceWhenInspecting;
            }
        }
        public void NotInteracting()
        {
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = startFocusDistance;
            }
        }
        public void ShowCursor() // Cursor'ı etkinleştir veya devre dışı bırak
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void HideCursor() // Cursor'ı etkinleştir veya devre dışı bırak
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}