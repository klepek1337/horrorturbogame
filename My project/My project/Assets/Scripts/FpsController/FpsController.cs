namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Unity.Cinemachine;

    [RequireComponent(typeof(CharacterController))]
    public class FpsController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 4.0f;
        public float sprintSpeed = 7.0f;
        public float rotationSpeed = 1.0f;
        public float accelerationRate = 10.0f;
        public float decelerationRate = 10f;

        [Header("Jump Settings")]
        public float jumpHeight = 2f;
        public float gravity = -20f;
        public float jumpCooldown = 0.2f;

        [Header("Grounded Settings")]
        public float groundedOffset = .85f;
        public float groundedRadius = 0.3f;
        public LayerMask groundLayers;

        [Header("Camera Settings")]
        public CinemachineCamera virtualCamera;
        public float maxCameraPitch = 70f;
        public float minCameraPitch = -70f;

        [Header("Headbob Settings")]
        public CinemachineBasicMultiChannelPerlin headBob;
        public float headBobAcceleration = 10f;
        public float idleBobAmp = .5f;
        public float idleBobFreq = 1f;
        public float walkBobAmp = 3f;
        public float walkBobFreq = 1f;
        public float sprintBobAmp = 4f;
        public float sprintBobFreq = 3f;

        [Header("Interact Settings")]
        public bool isInteracting = false;

        private CharacterController characterController;
        private FpsAssetsInputs _input;

        private Vector3 velocity;
        private bool isGrounded;
        private float jumpCooldownTimer;
        private float cameraPitch;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            var playerInput = GetComponent<PlayerInput>();
            _input = GetComponent<FpsAssetsInputs>();
        }

        private void Start()
        {
            if (virtualCamera == null)
            {
                Debug.LogError("Cinemachine Virtual Camera is not assigned.");
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleGravity();
            HandleJumping();
            GroundedCheck();
        }
        private void LateUpdate()
        {
            HandleRotation();
        }
        private void HandleMovement()
        {
            if (isInteracting)
            {
                _input.move = Vector2.zero;
                velocity = Vector3.zero;

                headBob.AmplitudeGain = idleBobAmp;
                headBob.FrequencyGain = idleBobFreq;
                return;
            }

            HeadBob();

            Vector2 input = _input.move;
            Vector3 moveDirection = transform.right * input.x + transform.forward * input.y;

            float targetSpeed = _input.sprint ? sprintSpeed : walkSpeed;

            if (moveDirection != Vector3.zero)
            {
                velocity.x = Mathf.Lerp(velocity.x, targetSpeed * moveDirection.x, Time.deltaTime * accelerationRate);
                velocity.z = Mathf.Lerp(velocity.z, targetSpeed * moveDirection.z, Time.deltaTime * accelerationRate);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * decelerationRate);
                velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * decelerationRate);
            }

            characterController.Move(new Vector3(velocity.x, 0, velocity.z) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (isInteracting) { return; }

            Vector2 lookInput = _input.look;
            cameraPitch += lookInput.y * rotationSpeed;
            cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

            virtualCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
            transform.Rotate(Vector3.up * lookInput.x * rotationSpeed);
        }
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void HandleGravity()
        {
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(Vector3.up * velocity.y * Time.deltaTime);
        }

        private void HandleJumping()
        {
            if (jumpCooldownTimer > 0)
            {
                jumpCooldownTimer -= Time.deltaTime;
            }

            if (isGrounded)
            {
                if (_input.jump && jumpCooldownTimer <= 0)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    jumpCooldownTimer = jumpCooldown;
                }
            }
            else
            {
                _input.jump = false;
            }
        }

        private void HeadBob()
        {
            float moveMagnitude = _input.move.magnitude; // Hareket miktarını hesapla
            float targetAmp = moveMagnitude > 0 ? (_input.sprint ? sprintBobAmp : walkBobAmp) : idleBobAmp;
            float targetFreq = moveMagnitude > 0 ? (_input.sprint ? sprintBobFreq : walkBobFreq) : idleBobFreq;

            headBob.AmplitudeGain = Mathf.Lerp(headBob.AmplitudeGain, targetAmp, Time.deltaTime * headBobAcceleration);
            headBob.FrequencyGain = Mathf.Lerp(headBob.FrequencyGain, targetFreq, Time.deltaTime * headBobAcceleration);
        }
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
        }
    }
}