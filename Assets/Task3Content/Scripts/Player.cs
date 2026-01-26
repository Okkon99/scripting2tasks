using Unity.Burst.Intrinsics;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace GameTask3
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [Header("")]
        [Header("Mouse/Aim/Look Settings")]

        // Mouse/Aim/Look
        [SerializeField] private float lookSensitivity;
        [SerializeField] private float mouseScrollSensitivity;

        [Header("Movement Settings")]
        [SerializeField] private float groundSpeed;
        [SerializeField] private float airSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float sprintSpeedPercent;

        [Header("Jump/Air control Settings")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpCutMultiplier;
        [SerializeField] private float coyoteTimer;


        [Header("Friction Settings")]
        [SerializeField] private float groundFriction;
        [SerializeField] private float airFriction;

        [Header("Camera Settings")]
        [SerializeField] private float cameraDistanceMin;
        [SerializeField] private float cameraDistanceMax;

        [Header("Dependencies")]
        [SerializeField] private CinemachineFollow cinemachineFollow;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CapsuleCollider playerCollider;
        [SerializeField] private LayerMask floorLayer;


        //camera
        float cameraPitch = 0.0f;
        float cameraDistance = 15.0f;
        float targetCameraDistance = 15.0f;

        //player
        bool isGrounded;
        float currentSpeed;
        Vector3 desiredVelocity;
        Vector2 moveInput;
        bool jumpPressed;
        bool jumpHeld;
        bool canJump;
        bool isJumping;
        float coyoteTime;

        bool sprintHeld;

        Vector3 groundNormal;
        RaycastHit groundHit;

        public bool hasKey;


        private void Awake()
        {

        }


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // CACHE INPUTS
            var input = GameManager.instance.Input.Gameplay;

            moveInput = input.Move.ReadValue<Vector2>();
            jumpHeld = input.Jump.IsPressed();
            sprintHeld = input.Sprint.IsPressed();

            if (input.Jump.triggered)
            {
                jumpPressed = true;
            }
            // CACHE INPUTS END

            { // CAMERA MOVEMENT
                Vector2 lookDelta = input.Look.ReadValue<Vector2>();
                Vector2 scrollDelta = input.Scroll.ReadValue<Vector2>();

                float mouseX = lookDelta.x * lookSensitivity;
                float mouseY = lookDelta.y * lookSensitivity;
                float mouseScroll = scrollDelta.y;

                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    //Zoom
                    targetCameraDistance -= mouseScroll * mouseScrollSensitivity;
                    targetCameraDistance = Mathf.Clamp(
                        targetCameraDistance,
                        cameraDistanceMin,
                        cameraDistanceMax
                    );

                    cameraDistance = Mathf.Lerp(
                        cameraDistance,
                        targetCameraDistance,
                        1f - Mathf.Exp(-10f * Time.deltaTime)
                    ); // i want to be extremely clear that this worked fine with just a step by step zoom,
                       // but i really wanted to see how you do a "smooth" variant.
                       // i couldnt figure it out but used ChatGPT in this instance.
                       // i still learned from it, in my opinion. I dont fully understand Mathf stuff yet.


                    //Yaw
                    transform.Rotate(Vector3.up * mouseX);
                    
                    //Pitch
                    cameraPitch -= mouseY;
                    cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);

                    float pitchRad = cameraPitch * Mathf.Deg2Rad;

                    cinemachineFollow.FollowOffset = new Vector3(
                        0.0f,
                        Mathf.Sin(pitchRad) * cameraDistance,
                        -Mathf.Cos(pitchRad) * cameraDistance
                    );
                }

            } // CAMERA MOVEMENT END

        }

        private void FixedUpdate()
        {
            isGrounded = CheckGrounded(out groundHit);
            groundNormal = isGrounded ? groundHit.normal : Vector3.up;



            GetDesiredVelocity();

            ApplyAccelVector();

            ApplyFriction();

            //Jump
            if (isGrounded)
            {
                ResetCoyoteTime();
                isJumping = false;
            }
            else
            {
                CoyoteTimeCountdown();
            }

            canJump = isGrounded || coyoteTime > 0f;

            if (jumpPressed)
            {
                if (canJump)
                {
                    Jump();
                }
                jumpPressed = false;
            }

            if (!jumpHeld && isJumping && rb.linearVelocity.y > 0f)
            {
                JumpCancel();
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Collectible"))
            {
                CollectItem(other.gameObject);
            }
        }

        private void CollectItem(GameObject item)
        {
            hasKey = true;
            Destroy(item);
        }

        private void ResetCoyoteTime()
        {
            coyoteTime = coyoteTimer;
        }
        private void CoyoteTimeCountdown()
        {
            coyoteTime -= Time.fixedDeltaTime;
            coyoteTime = Mathf.Max(coyoteTime, 0f);
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
            coyoteTime = 0f;
            isJumping = true;
        }
        private void JumpCancel()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, rb.linearVelocity.z);

            isJumping = false;
        }

        private void GetDesiredVelocity()
        {
            Vector3 inputDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

            if (inputDirection.sqrMagnitude > 1f)
            {
                inputDirection.Normalize();
            }

            float speed = isGrounded ? groundSpeed : airSpeed;

            if (isGrounded)
            {
                if (sprintHeld)
                {
                    speed *= 1f + sprintSpeedPercent / 100f;
                }
                inputDirection = Vector3.ProjectOnPlane(inputDirection, groundHit.normal).normalized;
            }
            desiredVelocity = inputDirection * speed;
        }

        private void ApplyAccelVector()
        {
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            Vector3 velocityDelta = desiredVelocity - currentHorizontal;

            float maxAccel = acceleration * Time.fixedDeltaTime;
            Vector3 accelVector = Vector3.ClampMagnitude(velocityDelta, maxAccel);
            rb.linearVelocity += accelVector;
        }

        private void ApplyFriction()
        {
            rb.linearDamping = isGrounded ? groundFriction : airFriction;
        }

        private bool CheckGrounded(out RaycastHit hit)
        {
            float radius = playerCollider.radius * 0.95f;
            float height = playerCollider.height * 0.5f - radius;

            Vector3 center = transform.position + playerCollider.center;
            Vector3 top = center + Vector3.up * height;
            Vector3 bottom = center - Vector3.up * height;

            float castDistance = 0.15f;

            return Physics.CapsuleCast(top, bottom, radius, Vector3.down, out hit, castDistance, floorLayer, QueryTriggerInteraction.Ignore);
        }
    }
}