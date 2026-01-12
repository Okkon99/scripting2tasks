using ScriptGame;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

class GameRules
{
    public int game_level = 1;
    public float max_mana = 100.0f;
    public float mana_upgrade_cost = 100.0f;
    public float max_mana_purchase_increase = 50.0f;
    public float player_mana_generated_per_second = 20.0f;
    public float player_mana_boost_per_level = 2.5f;
}



public class Game : MonoBehaviour
{
    public InputAction player_attack;
    public InputAction player_levelup;
    public InputAction player_manaupgrade;
    public InputAction player_resetgame;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    List<EnemyScript> enemies = new List<EnemyScript>();
    GameRules game_rules = new GameRules();

    float enemy_spawn_timer = 1.0f;
    float game_level_timer = 6.0f;

    float playerMana;
    float maxMana = 100.0f;

    int gameLevel = 1;
    int playerLevel = 1;
    float levelUpCost;
    string playerTitle = "test";
    GameObject lossEnemy;
    GameObject playerObject;

    List<Vector2Int> freeCells = new List<Vector2Int>();

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
            "Winner!"
    };

    void Start()
    {
        if (playerPrefab != null)
        {
            playerObject = Instantiate(playerPrefab, new Vector3(3f, 0.75f, 4f), transform.rotation);
        }

        PopulateValidCells();

        UpdatePlayerTitle();
    }

    void OnEnable()
    {
        player_attack.Enable();
        player_levelup.Enable();
        player_manaupgrade.Enable();
        player_resetgame.Enable();
    }

    void Update()
    {
        if (gameEnded)
        {
            if (player_resetgame.triggered)
            {
                ResetGame();
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
        {
            WinEvent();
        }


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

    //GUI Placeholder
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


    //   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods
    public void WinEvent()
    {
        Debug.Log("YOU WIN!!!!!");
        gameEnded = true;
    }

    public void LoseEvent()
    {
        Debug.Log("You lost");
        lossEnemy = Instantiate(enemyPrefab, new Vector3(3, 1, 4), transform.rotation);
        Destroy(playerObject);
        gameEnded = true;
    }

    public void ResetGame()
    {
        gameLevel = 1;
        playerLevel = 1;
        playerMana = 0f;
        maxMana = 100f;
        UpdatePlayerTitle();

        foreach (EnemyScript enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
        Destroy(lossEnemy.gameObject);
        PopulateValidCells();

        if (playerPrefab != null)
        {
            playerObject = Instantiate(playerPrefab, new Vector3(3f, 0.75f, 4f), transform.rotation);
        }

        gameEnded = false;
    }

    public void PopulateValidCells()
    {
        for (int x = 6; x < 9; x++)
        {
            for (int z = 3; z < 6; z++)
            {
                freeCells.Add(new Vector2Int(x, z));
            }
        }
    }

    public void OnGameLevelIncreaseEvent()
    {
        gameLevel++;
    }

    public void OnEnemySpawnEvent()
    {
        if (freeCells.Count == 0)
        {
            LoseEvent();
            return;
        }

        int index = RandomNumberGenerator.GetInt32(0, freeCells.Count);
        Vector2Int cell = freeCells[index];

        freeCells.RemoveAt(index);

        Vector3 worldPos = new Vector3(cell.x, 1f, cell.y);
        GameObject enemyObject = Instantiate(enemyPrefab, worldPos, transform.rotation);

        EnemyScript enemy = enemyObject.GetComponent<EnemyScript>();
        enemy.GridCell = cell;

        enemies.Add(enemy);



        return;
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
        playerTitle = playerTitles[playerLevel - 1];
    }

    public void OnPlayerInputAttack()
    {
        Debug.Log("input1");

        if (enemies.Count > 0)
        {
            EnemyScript oldestEnemy = enemies[0];

            if (oldestEnemy.hp <= playerMana)
            {
                playerMana -= oldestEnemy.hp;
                enemies.Remove(oldestEnemy);
                freeCells.Add(oldestEnemy.GridCell);
                Destroy(oldestEnemy.gameObject);
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
