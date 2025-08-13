using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public class ScriptProgram
    {
        static GameRules game_rules = new GameRules();
        static Player player = new Player();
        static List<Enemy> enemies = new List<Enemy>();

        public static void Start()
        {
            Debug.Log("Start has been called!");

            // reset variables if you have reload domain off!

        }

        public static void Update()
        {
        }

        public static void OnGUI()
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

        public static void OnGameLevelIncreaseEvent()
        {
        }

        public static void OnEnemySpawnEvent()
        {
        }

        public static void PlayerInput1()
        {
            Debug.Log("input1");
        }

        public static void PlayerInput2()
        {
            Debug.Log("input2");
        }

        public static void PlayerInput3()
        {
            Debug.Log("input3");
        }
    }
}