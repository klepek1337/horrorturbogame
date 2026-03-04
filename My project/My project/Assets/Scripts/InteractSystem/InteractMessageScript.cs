namespace FpsHorrorKit
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class InteractMessageScript : MonoBehaviour
    {
        public static InteractMessageScript Instance { get; private set; }

        [SerializeField] private CanvasGroup interactMessageCanvasGroup;
        [SerializeField] private TextMeshProUGUI interatMessageText;
        [SerializeField] private float displayTime = 5f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowMessage(string message, float displayTime = 5f)
        {
            interatMessageText.text = message;
            StopAllCoroutines();
            StartCoroutine(ShowInfoMessage());
        }


        IEnumerator ShowInfoMessage()
        {
            float duration = 0;
            interactMessageCanvasGroup.alpha = 0;
            interactMessageCanvasGroup.gameObject.SetActive(true);
            while (duration <= 1)
            {
                duration += Time.deltaTime / (displayTime / 2);
                interactMessageCanvasGroup.alpha = duration;
                yield return null;
            }
            while (duration >= 0)
            {
                duration -= Time.deltaTime / (displayTime / 2);
                interactMessageCanvasGroup.alpha = duration;
                yield return null;
            }
            interatMessageText.text = "";
            interactMessageCanvasGroup.gameObject.SetActive(false);
        }
    }
}