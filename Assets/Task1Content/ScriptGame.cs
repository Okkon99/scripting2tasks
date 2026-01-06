using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptGame
{
    class GameRules
    {
    }

    class Player
    {
    }

    class Enemy
    {
    }

    public class ScriptGame : MonoBehaviour
    {
        GameRules game_rules = new GameRules();
        Player player = new Player();
        List<Enemy> enemies = new List<Enemy>();

        public InputAction player_input_1;
        public InputAction player_input_2;
        public InputAction player_input_3;

        float enemy_spawn_timer = 1.0f;
        float game_level_timer = 6.0f;

        public void Start()
        {
            Debug.Log("Start has been called!");

            // reset variables if you have reload domain off!

        }

        void OnEnable()
        {
            player_input_1.Enable();
            player_input_2.Enable();
            player_input_3.Enable();
        }

        public void Update()
        {
            enemy_spawn_timer -= Time.deltaTime;
            game_level_timer -= Time.deltaTime;

            if (enemy_spawn_timer <= 0.0f)
            {
                OnEnemySpawnEvent();

                enemy_spawn_timer += RandomNumberGenerator.GetInt32(4, 6);
            }
            if (game_level_timer <= 0.0f)
            {
                OnGameLevelIncreaseEvent();

                game_level_timer += RandomNumberGenerator.GetInt32(4, 6);
            }

            if (player_input_1.triggered)
            {
                OnPlayerInput1();
            }
            if (player_input_2.triggered)
            {
                OnPlayerInput2();
            }
            if (player_input_3.triggered)
            {
                OnPlayerInput3();
            }
        }

        public void OnGUI()
        {
            float t = Time.time;

            GUI.Box(new Rect(0, 0, 250, 170), "");
            GUI.Label(new Rect(10, 10, 200, 20), "ScriptGame data");

            string title = "";
            int game_level = 0;
            int player_level = 0;
            float max_mana = 0;
            float levelup_cost = 0;
            float player_mana = 0;
            int enemy_count = 0;

            GUI.Label(new Rect(10, 30, 300, 20), String.Format("game level {0}", game_level));
            GUI.Label(new Rect(10, 50, 300, 20), String.Format("player: level {0} ({1})", player_level, title));
            GUI.Label(new Rect(10, 90, 300, 20), String.Format("max_mana: {0}", max_mana));
            GUI.Label(new Rect(10, 110, 300, 20), String.Format("levelup cost: {0}", levelup_cost));
            GUI.Label(new Rect(10, 130, 300, 20), String.Format("mana: {0}", player_mana));
            GUI.Label(new Rect(10, 150, 300, 20), String.Format("enemies: {0}", enemy_count));
        }

        public void OnGameLevelIncreaseEvent()
        {
        }

        public void OnEnemySpawnEvent()
        {
        }

        public void OnPlayerInput1()
        {
            Debug.Log("input1");
        }

        public void OnPlayerInput2()
        {
            Debug.Log("input2");
        }

        public void OnPlayerInput3()
        {
            Debug.Log("input3");
        }
    }
}