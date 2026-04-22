using UnityEngine;

public class MonsterDamageByDistance : MonoBehaviour
{
    private const float DefaultDamageDistance = 1.2f;
    private const int DamageAmountPerHit = 1;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private float damageDistance = DefaultDamageDistance;
    [SerializeField] private AudioSource monsterAudioSource;
    [SerializeField] private AudioClip hitSound;

    private Vector3 monsterStartPosition;
    private Quaternion monsterStartRotation;
    private bool hasDamagedPlayerInThisApproach = false;

    private void Start()
    {
        monsterStartPosition = transform.position;
        monsterStartRotation = transform.rotation;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        if (playerLives == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= damageDistance && !hasDamagedPlayerInThisApproach)
        {
            hasDamagedPlayerInThisApproach = true;

            playerLives.TakeDamage(DamageAmountPerHit);
            PlayHitSound();
            ReturnMonsterToStartPoint();
        }
    }

    private void PlayHitSound()
    {
        if (monsterAudioSource == null)
        {
            return;
        }

        if (hitSound == null)
        {
            return;
        }

        monsterAudioSource.PlayOneShot(hitSound);
    }

    private void ReturnMonsterToStartPoint()
    {
        transform.position = monsterStartPosition;
        transform.rotation = monsterStartRotation;
        hasDamagedPlayerInThisApproach = false;
    }
}