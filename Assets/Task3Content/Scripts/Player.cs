using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace GameTask3
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float sensitivity;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpHeight;
        [Header("Dependencies")]
        [SerializeField] private CinemachineFollow cinemachineFollow;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Rigidbody rb;

        
        private CharacterController controller;
        
        Vector3 velocity;


        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var input = GameManager.instance.Input.Gameplay;

            if (input.Jump.triggered)
            {
                rb.linearVelocity = velocity;
                Debug.Log("Successfully detected spacebar input");
            }





            { // CAMERA MOVEMENT
                Vector2 lookDelta = input.Look.ReadValue<Vector2>();

                float mouseX = lookDelta.x * sensitivity;
                float mouseY = lookDelta.y * sensitivity;

                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    transform.Rotate(new Vector3(0.0f, mouseX, 0.0f));
                    /*
                    cameraPitch -= mouse_y * 0.035f;
                    cameraPitch = Mathf.Clamp(
                        cameraPitch,
                        -Mathf.PI / 2.0f + 0.1f,
                        Mathf.PI / 2.0f - 0.1f
                    );

                    cinemachineFollow.FollowOffset = new Vector3(
                        0.0f,
                        cameraDistance * Mathf.Sin(cameraPitch),
                        -cameraDistance * Mathf.Cos(cameraPitch)
                    );*/
                }

            } // CAMERA MOVEMENT
        }
    }
}