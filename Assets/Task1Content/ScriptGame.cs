using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptGame
{
    class GameRules
    {
        public int game_level = 1;
        public float max_mana = 100.0f;
        public float mana_upgrade_cost = 100.0f;
        public float max_mana_purchase_increase = 50.0f;
        public float player_mana_generated_per_second = 20.0f;
        public float player_mana_boost_per_level = 2.5f;
    }

    class Player
    {
        public float mana = 0.0f;
        public int level = 1;
        //    a label is displayed for each player level (1 to 9) - private, private 1st class, corporal, sergeant, major, officer, captain, colonel, general
    }

    class Enemy
    {
        public float hp = 100.0f;
    }

    public class ScriptGame : MonoBehaviour
    {
        GameRules game_rules = new GameRules();
        Player player = new Player();
        List<Enemy> enemies = new List<Enemy>();

        public InputAction player_attack;
        public InputAction player_levelup;
        public InputAction player_manaupgrade;
        public InputAction player_resetgame;


        float enemy_spawn_timer = 1.0f;
        float game_level_timer = 6.0f;

        float playerMana;
        float maxMana = 100.0f;

        int gameLevel = 1;
        int playerLevel = 1;
        float levelUpCost;
        string playerTitle = "test";

        bool gameEnded = false;

        string[] playerTitles = new string[]
        {
            "Private",
            "Private 1st Class",
            "Corporal",
            "Sergeant",
            "Major",
            "Officer",
            "Captain",
            "Colonel",
            "General",
        };

        public void Start()
        {
            Debug.Log("Start has been called!");

            UpdatePlayerTitle();
            // reset variables if you have reload domain off!

        }

        void OnEnable()
        {
            player_attack.Enable();
            player_levelup.Enable();
            player_manaupgrade.Enable();
            player_resetgame.Enable();
        }

        public void Update()
        {
            if (gameEnded)
            {
                if (player_resetgame.triggered)
                {
                    Debug.Log("if statement successful");
                    gameLevel = 1;
                    playerLevel = 1;
                    playerMana = 0f;
                    maxMana = 100f;
                    enemies.Clear();
                    UpdatePlayerTitle();

                    gameEnded = false;
                }
                return;
            }

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

            ManaRegeneration();
            CalculateLevelUpCost();

            if (playerLevel == 10)
                WinEvent();


            //Inputs
            if (player_attack.triggered)
            {
                OnPlayerInputAttack();
            }
            if (player_levelup.triggered)
            {
                OnPlayerInputLevelUp();
            }
            if (player_manaupgrade.triggered)
            {
                OnPlayerInputUpgrade();
            }
        }

        public void OnGUI()
        {
            float t = Time.time;

            GUI.Box(new Rect(0, 0, 250, 170), "");
            GUI.Label(new Rect(10, 10, 200, 20), "ScriptGame data");


            GUI.Label(new Rect(10, 30, 300, 20), String.Format("game level {0}", gameLevel));
            GUI.Label(new Rect(10, 50, 300, 20), String.Format("player: level {0} ({1})", playerLevel, playerTitle));
            GUI.Label(new Rect(10, 90, 300, 20), String.Format("max_mana: {0}", maxMana));
            GUI.Label(new Rect(10, 110, 300, 20), String.Format("levelup cost: {0:0}", levelUpCost));
            GUI.Label(new Rect(10, 130, 300, 20), String.Format("mana: {0:0}", playerMana));
            GUI.Label(new Rect(10, 150, 300, 20), String.Format("enemies: {0}", enemies.Count));
        }

        public void WinEvent()
        {
            Debug.Log("YOU WIN!!!!!");
            gameEnded = true;
        }

        public void LoseEvent()
        {
            Debug.Log("You lost");
            gameEnded = true;
        }

        public void OnGameLevelIncreaseEvent()
        {
            gameLevel++;
        }

        public void OnEnemySpawnEvent()
        {
            enemies.Add(new Enemy());

            if (enemies.Count >= 10)
            {
                LoseEvent();
            }
        }

        public void ManaRegeneration()
        {
            float regenPerSecond = game_rules.player_mana_generated_per_second + game_rules.player_mana_boost_per_level * playerLevel;
            
            if (playerMana < maxMana)
            {
                playerMana += regenPerSecond * Time.deltaTime;
                playerMana = Mathf.Min(playerMana, maxMana);
            }
        }

        public float CalculateLevelUpCost()
        {
            levelUpCost = (100f + 5f * gameLevel + 10f * enemies.Count);

            return levelUpCost;
        }

        public void UpdatePlayerTitle()
        {
            playerTitle = playerTitles[playerLevel-1];
        }

        public void OnPlayerInputAttack()
        {
            Debug.Log("input1");

            if (enemies.Count > 0)
            {
                Enemy oldestEnemy = enemies[0];

                if (oldestEnemy.hp <= playerMana)
                {
                    playerMana -= oldestEnemy.hp;
                    enemies.Remove(oldestEnemy);
                }
                else
                {
                    oldestEnemy.hp -= playerMana;
                    playerMana = 0f;
                }
            }

        }

        public void OnPlayerInputLevelUp()
        {
            Debug.Log("input2");

            if (playerMana >= levelUpCost)
            {
                playerMana -= levelUpCost;
                playerLevel++;

                UpdatePlayerTitle();
            }
        }

        public void OnPlayerInputUpgrade()
        {
            Debug.Log("input3");

            if (playerMana >= game_rules.mana_upgrade_cost)
            {
                playerMana -= game_rules.mana_upgrade_cost;
                maxMana += game_rules.max_mana_purchase_increase;
            }
            
        }
    }
}