using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FinalCube : MonoBehaviour
{
    CharacterController controller;

    Vector2 input_move;

    public float moveSpeed = 7.0f;

    GameObject target;
    Vector3 targetStartPosition;
    GameObject aim;
    GameObject flail;
    public GameObject ballPrefab;

    int subtask_focused_idx = 0;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        target = GameObject.FindGameObjectWithTag("Target");
        targetStartPosition = target.transform.position;
        aim = transform.GetChild(0).gameObject;
        flail = transform.GetChild(1).gameObject;
        flail.SetActive(false);
    }

    void Update()
    {
        controller.Move(new Vector3(input_move.x, 0.0f, input_move.y).normalized * moveSpeed * Time.deltaTime);
    }

    public void FlailLaunch(GameObject other)
    {
        Vector3 launch_vector = new Vector3(0.0f, 0.0f, 0.0f); // replace

        other.gameObject.GetComponent<Rigidbody>().AddForce(launch_vector);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector3 ball_spawn_position = new Vector3(0.0f, 0.0f, 0.0f); // replace
            GameObject ball = Instantiate(ballPrefab, ball_spawn_position, Quaternion.identity);

            Vector3 force_vector = new Vector3(0.0f, 0.0f, 0.0f); // replace
            ball.GetComponent<Rigidbody>().AddForce(force_vector);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input_move = context.ReadValue<Vector2>();
    }

    public void ResetTarget(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            target.transform.position = targetStartPosition;
            Rigidbody rigid_body = target.GetComponent<Rigidbody>();

            if (rigid_body.linearVelocity.magnitude > 0.0f)
            {
                rigid_body.linearVelocity = Vector3.zero;
                rigid_body.angularVelocity = Vector3.zero;
            }
        }
    }

    public void Flail(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            flail.SetActive(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            flail.SetActive(false);
        }
    }

    public void SubTask(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (context.control.name == "e")
            {
                subtask_focused_idx++;
            }
            else if (context.control.name == "q")
            {
                subtask_focused_idx--;
            }

            subtask_focused_idx = Math.Clamp(subtask_focused_idx, 0, 13);

            GameObject subtask_object = GameObject.Find("tasks").transform.GetChild(subtask_focused_idx).gameObject;

            Camera game_camera = Camera.allCameras[0];
            game_camera.transform.position = subtask_object.transform.position + new Vector3(-2.0f, 5.0f, -10.0f);
            game_camera.transform.LookAt(subtask_object.transform);
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 25, 600, 25), "");
        GUI.Label(new Rect(10, 30, 600, 20), "controls - space: shoot, v: flail, r: reset target, wasd: move, 1,2,3... camera jump to subtask");
    }
}
