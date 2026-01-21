using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameTask3
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float sensitivity;
        [SerializeField] private float mouseScrollSensitivity;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeedPercent;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float airControlStrength;
        [SerializeField] private float airControlDecay;


        [Header("Camera Settings")]
        [SerializeField] private float cameraDistanceMin;
        [SerializeField] private float cameraDistanceMax;

        [Header("World Settings")]
        [SerializeField] private float gravity;

        [Header("Dependencies")]
        [SerializeField] private CinemachineFollow cinemachineFollow;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private CharacterController controller;
        
        //camera
        float cameraPitch = 0.0f;
        float cameraDistance = 15.0f;
        float targetCameraDistance = 15.0f;


        //player
        Vector3 velocity;
        Vector3 verticalVelocity;
        Vector3 horizontalVelocity;
        float targetSpeed;
        float baseSpeed;
        public float currentAirControl;

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
            var input = GameManager.instance.Input.Gameplay;

            { // CAMERA MOVEMENT
                Vector2 lookDelta = input.Look.ReadValue<Vector2>();
                Vector2 scrollDelta = input.Scroll.ReadValue<Vector2>();

                float mouseX = lookDelta.x * sensitivity;
                float mouseY = lookDelta.y * sensitivity;
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

            } // CAMERA MOVEMENT

            { // PLAYER MOVEMENT
                Vector2 moveInput = input.Move.ReadValue<Vector2>();
                Vector3 inputDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

                baseSpeed = moveSpeed;
                bool isSprinting = controller.isGrounded && input.Sprint.IsPressed();
                targetSpeed = isSprinting ? baseSpeed * (1f + sprintSpeedPercent / 100f) : baseSpeed;


                bool canDoubleJump = false;


                if (controller.isGrounded)
                {
                    horizontalVelocity = inputDirection.normalized * targetSpeed;
                    currentAirControl = airControlStrength;

                    if (verticalVelocity.y < 0f)
                        verticalVelocity.y = -2f;

                    if (input.Jump.triggered)
                    {
                        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    }
                    canDoubleJump = true;
                }
                else
                {
                    horizontalVelocity += inputDirection.normalized * targetSpeed * currentAirControl * Time.deltaTime;

                    currentAirControl = Mathf.MoveTowards(currentAirControl, 0f, airControlDecay * Time.deltaTime);
                }
                /*
                if (input.Jump.triggered && canDoubleJump)
                {
                    canDoubleJump = false;
                    Debug.Log("doublejumped");
                }*/

                if (input.DiveCrouch.triggered)
                {
                    if (horizontalVelocity.x < 0.1f && (horizontalVelocity.z < 0.1f))
                    {
                        Debug.Log("Crouch!");

                    }
                    else
                    {
                        Debug.Log("Dive!");

                    }
                }

                verticalVelocity.y += gravity * Time.deltaTime;

                Vector3 finalMove = horizontalVelocity + Vector3.up * verticalVelocity.y;
                controller.Move(finalMove * Time.deltaTime);

            } // PLAYER MOVEMENT
        }

        public void PickupKey()
        {
            hasKey = true;
        }

        private void ExampleMethod()
        {
            //do stuff
        }
    }
}