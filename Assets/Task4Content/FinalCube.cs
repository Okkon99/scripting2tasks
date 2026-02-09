using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FinalCube : MonoBehaviour
{
    [SerializeField] float ballForce;
    [SerializeField] float flailForce;

    CharacterController controller;

    Vector2 inputMove;

    public float moveSpeed = 7.0f;

    [SerializeField] float rotateSpeed;

    GameObject target;
    Vector3 targetDir;
    Vector3 targetStartPosition;
    GameObject aim;
    GameObject flail;
    Vector3 flailToTargetDir;
    public GameObject ballPrefab;
    float currentAimRot;

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
        targetDir = (target.transform.position - transform.position).normalized;
        aim.transform.position = transform.position + (targetDir);

        currentAimRot += rotateSpeed * Time.deltaTime * 180f;
        aim.transform.LookAt(target.transform);
        aim.transform.Rotate(0, 0, currentAimRot);
        controller.Move(new Vector3(inputMove.x, 0.0f, inputMove.y).normalized * moveSpeed * Time.deltaTime);

        flailToTargetDir = (target.transform.position - flail.transform.position).normalized;
    }

    public void FlailLaunch(GameObject other)
    {
        Vector3 launchVector = flailToTargetDir * flailForce; // replace

        other.gameObject.GetComponent<Rigidbody>().AddForce(launchVector);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector3 ballSpawnPosition = aim.transform.position;
            GameObject ball = Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity);

            Vector3 forceVector = targetDir * ballForce;
            ball.GetComponent<Rigidbody>().AddForce(forceVector);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
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
