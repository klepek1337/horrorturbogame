namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PhotoCameraUpater : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private Item itemCamera;
        [SerializeField] private Image cameraBatteryImage;
        [SerializeField] private float batteryDecraseSpeed = .25f;

        private void Start()
        {
            cameraBatteryImage.fillAmount = itemCamera.energyLevel / 100f;
            if (itemCamera.energyLevel > 0)
            {
                itemCamera.isEnergyEnough = true;
            }
        }
        public void UpdateBattery(float energyLevel)
        {
            itemCamera.energyLevel += energyLevel;
            cameraBatteryImage.fillAmount = itemCamera.energyLevel / 100f;

            if (itemCamera.energyLevel > 100)
            {
                itemCamera.energyLevel = 100;
            }

        }
        private void LateUpdate()
        {
            if (itemCamera != null)
            {
                if (itemCamera.canUseItem)
                {
                    if (itemCamera.energyLevel > 0)
                    {
                        itemCamera.energyLevel -= Time.deltaTime * batteryDecraseSpeed;
                        if (cameraBatteryImage != null) { cameraBatteryImage.fillAmount = itemCamera.energyLevel / 100f; }

                        itemCamera.isEnergyEnough = true;
                        ItemUsageSystem.Instance._light.SetActive(true);
                    }
                    else
                    {
                        itemCamera.isEnergyEnough = false;
                        ItemUsageSystem.Instance._light.SetActive(false);
                    }
                }
            }
        }
    }
}