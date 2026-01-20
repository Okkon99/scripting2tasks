using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vertx.Debugging;
using Vertx.Debugging.Internal;

public class PlayerGuy : MonoBehaviour
{
    public UnityEvent<int> objective_completed_event;
    int objectives_completed = 0;

    public CinemachineFollow cinemachine_follow;
    CharacterController controller;
    Vector3 velocity;

    public float movement_speed = 5.0f;
    public float camera_distance_min = 5.0f;
    public float camera_distance_max = 15.0f;
    float camera_pitch = 0.0f;
    float camera_distance = 10.0f;

    public bool has_key = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        { // CURSOR

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        } // CURSOR

        { // CAMERA MOVEMENT

            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = Input.GetAxis("Mouse Y");
            float mouse_scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                camera_distance += -mouse_scroll * 3.0f;
                camera_distance = Math.Clamp(
                    camera_distance,
                    camera_distance_min,
                    camera_distance_max
                );

                transform.Rotate(new Vector3(0.0f, mouse_x, 0.0f));

                camera_pitch -= mouse_y * 0.035f;
                camera_pitch = Mathf.Clamp(
                    camera_pitch,
                    -Mathf.PI / 2.0f + 0.1f,
                    Mathf.PI / 2.0f - 0.1f
                );

                cinemachine_follow.FollowOffset = new Vector3(
                    0.0f,
                    camera_distance * Mathf.Sin(camera_pitch),
                    -camera_distance * Mathf.Cos(camera_pitch)
                );
            }

        } // CAMERA MOVEMENT

        { // MOVEMENT

            float forward = Input.GetAxis("Vertical");
            float right = Input.GetAxis("Horizontal");
            bool space_pressed = Input.GetKeyDown(KeyCode.Space);

            Vector3 movement_vector = transform.forward * forward + transform.right * right;

            if (movement_vector.magnitude > 0.0f)
            {
                if (movement_vector.magnitude > 1.0f)
                {
                    movement_vector.Normalize();
                }

                movement_vector *= movement_speed;
            }

            if (controller.isGrounded)
            {
                if (velocity.y < -2.0f)
                {
                    velocity.y = -2.0f;
                }

                if (space_pressed)
                {
                    velocity.y += 15.0f;
                }
            }

            velocity.y += Physics.gravity.y * 2.0f * Time.deltaTime;

            controller.Move((movement_vector + velocity) * Time.deltaTime);

        } // MOVEMENT

        if (Input.GetKeyDown(KeyCode.V))
        {
            TryInteract();
        }
    }

    public void PickupKey()
    {
        has_key = true;
        objectives_completed++;
    }

    void TryInteract()
    {
        float radius = 2.0f;
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in collisions)
        {
            Gate gate_collided_with = collider.GetComponent<Gate>();

            if (gate_collided_with)
            {
                bool opened_gate = gate_collided_with.TryOpenGate(this);

                if (opened_gate)
                {
                    has_key = false;
                    objectives_completed++;
                    objective_completed_event.Invoke(objectives_completed);
                }
            }
        }

        { // DEBUG
            Color color = Color.green;
            D.raw(new Shape.Sphere(transform.position, radius), color, 1.0f);
        } // DEBUG
    }

    public void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 250, 70), "");
        GUI.Label(new Rect(10, 10, 200, 20), "PlayerGuy");
        GUI.Label(new Rect(10, 30, 300, 20), String.Format("has key {0}", has_key));
        GUI.Label(new Rect(10, 50, 300, 20), String.Format("objectives completed {0}", objectives_completed));
    }
}
