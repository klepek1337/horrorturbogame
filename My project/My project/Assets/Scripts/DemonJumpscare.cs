using UnityEngine;

public class DemonJumpscare : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 1.2f;
    public AudioSource jumpscareSound;
    public GameObject jumpscareUI; // Image ou effet à afficher
    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
        {
            hasTriggered = true;

            // Son
            if (jumpscareSound != null)
                jumpscareSound.Play();

            // UI
            if (jumpscareUI != null)
                jumpscareUI.SetActive(true);

            // Optionnel : arrêter le jeu
            // Time.timeScale = 0f;
        }
    }
}
