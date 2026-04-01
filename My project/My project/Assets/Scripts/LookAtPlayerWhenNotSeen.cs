using UnityEngine;

public class LookAtPlayerWhenNotSeen : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 2f;
    public Camera playerCamera;

    void Update()
    {
        if (player == null || playerCamera == null) return;

        // Vérifie si le DemonDoll est dans le champ de vision du joueur
        Vector3 viewPos = playerCamera.WorldToViewportPoint(transform.position);

        bool isVisible =
            viewPos.x > 0 && viewPos.x < 1 &&
            viewPos.y > 0 && viewPos.y < 1 &&
            viewPos.z > 0;

        // S'il est visible → il ne bouge pas
        if (isVisible) return;

        // Sinon → il te regarde
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
