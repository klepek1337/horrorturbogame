using UnityEngine;

public class MoveWhenNotSeen : MonoBehaviour
{
    public Transform player;
    public Camera playerCamera;
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.5f;
    public float groundOffset = 0.05f; // Ajuste la hauteur des pieds

    void Update()
    {
        if (player == null || playerCamera == null) return;

        // Vérifie si le démon est visible
        Vector3 viewPos = playerCamera.WorldToViewportPoint(transform.position);
        bool isVisible =
            viewPos.x > 0 && viewPos.x < 1 &&
            viewPos.y > 0 && viewPos.y < 1 &&
            viewPos.z > 0;

        if (isVisible) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= stopDistance) return;

        // Direction horizontale uniquement (XZ)
        Vector3 targetPos = new Vector3(
            player.position.x,
            transform.position.y,   // On garde la hauteur actuelle
            player.position.z
        );

        Vector3 direction = (targetPos - transform.position).normalized;

        // Déplacement horizontal
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Coller au sol avec un raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y + groundOffset,
                transform.position.z
            );
        }
    }
}
