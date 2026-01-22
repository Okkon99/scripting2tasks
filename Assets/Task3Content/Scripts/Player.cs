using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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
        [SerializeField] private float airTimer;

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
        [SerializeField] private LayerMask floorLayer;


        //camera
        float cameraPitch = 0.0f;
        float cameraDistance = 15.0f;
        float targetCameraDistance = 15.0f;


        //player (old)
        Vector3 velocity;
        Vector3 verticalVelocity;
        Vector3 horizontalVelocity;
        float targetSpeed;
        float baseSpeed;

        //player
        bool isGrounded;
        float airTime;
        float currentSpeed;

        Vector3 desiredVelocity;
        Vector2 moveInput;
        bool jumpPressed;
        bool sprintHeld;


        //inventory
        bool hasKey = false;


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
                jumpPressed = input.Jump.triggered;
                sprintHeld = input.Sprint.IsPressed();
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
            //temporary grounded check. just a raycast down. if hit, isGrounded=true
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, floorLayer);
            if(!isGrounded)
                Debug.Log(isGrounded);

            Vector3 inputDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
            inputDirection = inputDirection.normalized;

            float speed = isGrounded ? groundSpeed : airSpeed;

            if (sprintHeld && isGrounded)
            {
                speed *= 1f + sprintSpeedPercent / 100f;
            }

            desiredVelocity = inputDirection * speed;

            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            Vector3 velocityDelta = desiredVelocity - currentHorizontal;

            float maxAccel = acceleration * Time.fixedDeltaTime;
            Vector3 accelVector = Vector3.ClampMagnitude(velocityDelta, maxAccel);
            rb.linearVelocity += accelVector;

            float friction = isGrounded ? groundFriction : airFriction;
            
            rb.linearDamping = isGrounded ? groundFriction : airFriction;
        }

        public void PickupKey()
        {
            hasKey = true;
        }
    }
}