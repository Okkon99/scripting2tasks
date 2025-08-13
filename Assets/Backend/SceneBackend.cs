using System;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneBackend : MonoBehaviour
{
    // (daniel) this class is only for stuff that helps demonstrating coding in this project, don't modify :)

    public InputAction player_input_1;
    public InputAction player_input_2;
    public InputAction player_input_3;

    float timer = 1.0f;
    float long_timer = 6.0f;

    void Start()
    {
#if UNITY_EDITOR
        SceneView view = SceneView.lastActiveSceneView;
        Camera scene_view_camera = view.camera;
        Camera game_camera = Camera.allCameras[0];
        game_camera.transform.SetPositionAndRotation(scene_view_camera.transform.position, scene_view_camera.transform.rotation);
#endif

        ScriptGame.ScriptProgram.Start();
    }

    void OnEnable()
    {
        player_input_1.Enable();
        player_input_2.Enable();
        player_input_3.Enable();
    }

    void OnGUI()
    {
        ScriptGame.ScriptProgram.OnGUI();
    }

    void Update()
    {
        ScriptGame.ScriptProgram.Update();

        timer -= Time.deltaTime;
        long_timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            ScriptGame.ScriptProgram.OnEnemySpawnEvent();
            timer += RandomNumberGenerator.GetInt32(4, 6);
        }
        if (long_timer <= 0.0f)
        {
            ScriptGame.ScriptProgram.OnGameLevelIncreaseEvent();
            long_timer += RandomNumberGenerator.GetInt32(4, 6);
        }

        if (player_input_1.triggered)
        {
            ScriptGame.ScriptProgram.PlayerInput1();
        }
        if (player_input_2.triggered)
        {
            ScriptGame.ScriptProgram.PlayerInput2();
        }
        if (player_input_3.triggered)
        {
            ScriptGame.ScriptProgram.PlayerInput3();
        }
    }
}
