using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace GameTask3
{
    public class Player : MonoBehaviour
    {
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

            if (input.Look.triggered)
            {
                Debug.Log("Successfully detected mouse input");
            }
        }
    }
}