namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.UI;

    public class FlashLightUpdater : MonoBehaviour
    {
        [SerializeField] private Item itemFlashLight;
        [SerializeField] private Image flashLightBatteryImage;
        [SerializeField] private float batteryDecraseSpeed = .25f;


        private void Start()
        {
            flashLightBatteryImage.fillAmount = itemFlashLight.energyLevel / 100f;
            if (itemFlashLight.energyLevel > 0)
            {
                itemFlashLight.isEnergyEnough = true;
            }
        }
        public void UpdateBattery(float energyLevel)
        {
            itemFlashLight.energyLevel += energyLevel;
            flashLightBatteryImage.fillAmount = itemFlashLight.energyLevel / 100f;

            if (itemFlashLight.energyLevel > 100)
            {
                itemFlashLight.energyLevel = 100;
            }
        }
        private void LateUpdate()
        {
            if (itemFlashLight != null)
            {
                if (itemFlashLight.isUsingItem)
                {
                    if (itemFlashLight.energyLevel > 0)
                    {
                        itemFlashLight.energyLevel -= Time.deltaTime * batteryDecraseSpeed;
                        if (flashLightBatteryImage != null) { flashLightBatteryImage.fillAmount = itemFlashLight.energyLevel / 100f; }

                        itemFlashLight.isEnergyEnough = true;
                        ItemUsageSystem.Instance._light.SetActive(true);
                    }
                    else
                    {
                        itemFlashLight.isEnergyEnough = false;
                        ItemUsageSystem.Instance._light.SetActive(false);
                    }
                }
            }
        }
    }
}