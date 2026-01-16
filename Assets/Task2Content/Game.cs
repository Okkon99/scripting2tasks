using ScriptGame;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;

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
    public GameObject manaPrefab;
    public GameObject maxManaPrefab;
    public GameObject levelUpCostPrefab;
    public GameObject enemyHPPrefab;
    public TMP_Text playerAreaLabel;

    List<EnemyScript> enemies = new List<EnemyScript>();
    GameRules game_rules = new GameRules();

    float enemy_spawn_timer = 1.0f;
    float game_level_timer = 6.0f;


    int gameLevel = 1;
    float levelUpCost;
    string playerTitle = "test";
    GameObject lossEnemy;
    GameObject playerObject;
    GameObject manaObject;
    GameObject maxManaObject;
    GameObject levelUpCostObject;
    GameObject enemyHPObject;
    Player player;


    List<Vector2Int> freeCells = new List<Vector2Int>();

    bool gameEnded = false;

    string[] playerTitles = new string[]
    {
            "Private (1)",
            "Private 1st Class (2)",
            "Corporal (3)",
            "Sergeant (4)",
            "Major (5)",
            "Officer (6)",
            "Captain (7)",
            "Colonel (8)",
            "General (9)",
            "Winner!"
    };

    void Start()
    {
        SpawnPlayer();
        PopulateValidCells();
        UpdatePlayerTitle();
        SpawnMaxManaObj();

        manaObject =        Instantiate(manaPrefab,         new Vector3(2f, 0f, 2f), Quaternion.Euler(1f, 0f, 0f));
        levelUpCostObject = Instantiate(levelUpCostPrefab,  new Vector3(3f, 0f, 2f), Quaternion.Euler(0.2f, 0f, 0f));
        enemyHPObject =     Instantiate(enemyHPPrefab,      new Vector3(3f, 0f, 2f), Quaternion.Euler(0.2f, 0f, 0f));
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
                UpdatePlayerTitle();
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

        if (player.level == 10)
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
        GUI.Label(new Rect(10, 50, 300, 20), String.Format("player: level {0} ({1})", player.level, playerTitle));
        GUI.Label(new Rect(10, 90, 300, 20), String.Format("max_mana: {0}", player.maxMana));
        GUI.Label(new Rect(10, 110, 300, 20), String.Format("levelup cost: {0:0}", levelUpCost));
        GUI.Label(new Rect(10, 130, 300, 20), String.Format("mana: {0:0}", player.mana));
        GUI.Label(new Rect(10, 150, 300, 20), String.Format("enemies: {0}", enemies.Count));
    }


    //   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods

    //   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods

    //   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods   Methods
    
    void SpawnPlayer()
    {
        playerObject = Instantiate(playerPrefab, new Vector3(3f, 0.75f, 4f), transform.rotation);
        player = playerObject.GetComponent<Player>();
    }

    void SpawnMaxManaObj()
    {
        maxManaObject = Instantiate(maxManaPrefab, new Vector3(3f, 0f, 2f), Quaternion.Euler(0.2f, 0f, 0f));
    }

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

        UpdatePlayerTitle();

        foreach (EnemyScript enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        enemies.Clear();

        if (lossEnemy)
            Destroy(lossEnemy);

        if (playerObject)
            Destroy(playerObject);
        
        PopulateValidCells();
        SpawnPlayer();

        if (maxManaObject)
        {
            Destroy(maxManaObject);
            SpawnMaxManaObj();
        }


        player.level = 1;
        player.mana = 0f;
        player.maxMana = 100f;

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
        if (player == null) // idk if this is needed anymore, but in a previous version of this setup,
            return;         // not having this broke everything so im leaving it

        float regenPerSecond = game_rules.player_mana_generated_per_second + game_rules.player_mana_boost_per_level * player.level;

        if (manaObject)
        {
            Transform manaVisual = manaObject.transform.GetChild(0);

            Vector3 scale = manaVisual.transform.localScale;
            Vector3 position = manaVisual.transform.localPosition;
            scale.y = player.mana / 200;
            position.x = player.mana / 200;

            manaVisual.transform.localScale = scale;
            manaVisual.transform.localPosition = position;
        }

        if (player.mana < player.maxMana)
        {
            player.mana += regenPerSecond * Time.deltaTime;
            player.mana = Mathf.Min(player.mana, player.maxMana);
        }
    }

    public float CalculateLevelUpCost()
    {
        levelUpCost = (100f + 5f * gameLevel + 10f * enemies.Count);

        if (levelUpCostObject)
        {
            Vector3 position = levelUpCostObject.transform.localPosition;

            position.x = levelUpCost / 100 + 2;

            levelUpCostObject.transform.localPosition = position;
        }

        return levelUpCost;
    }

    public void UpdatePlayerTitle()
    {
        playerTitle = playerTitles[player.level - 1];

        if (!playerAreaLabel)
            return;

        playerAreaLabel.text = $"Player Rank: \n{playerTitle}";
    }

    public void OnPlayerInputAttack()
    {
        Debug.Log("input1");

        if (enemies.Count > 0)
        {
            EnemyScript oldestEnemy = enemies[0];

            if (oldestEnemy.hp <= player.mana)
            {
                player.mana -= oldestEnemy.hp;
                enemies.Remove(oldestEnemy);
                freeCells.Add(oldestEnemy.GridCell);
                Destroy(oldestEnemy.gameObject);
                
                if (enemyHPObject)
                {
                    enemyHPObject.transform.position = new Vector3(3f, 0f, 2f);
                }
            }
            else
            {
                oldestEnemy.hp -= player.mana;
                player.mana = 0f;

                if (enemyHPObject)
                {
                    Vector3 position = enemyHPObject.transform.localPosition;

                    position.x = oldestEnemy.hp / 100 + 2;

                    enemyHPObject.transform.localPosition = position;
                }
            }
        }
    }

    public void OnPlayerInputLevelUp()
    {
        Debug.Log("input2");

        if (player.mana >= levelUpCost)
        {
            player.mana -= levelUpCost;
            player.level++;

            UpdatePlayerTitle();
        }
    }

    public void OnPlayerInputUpgrade()
    {
        Debug.Log("input3");

        if (player.mana >= game_rules.mana_upgrade_cost)
        {
            player.mana -= game_rules.mana_upgrade_cost;
            player.maxMana += game_rules.max_mana_purchase_increase;

            if (maxManaObject)
            {
                Vector3 position = maxManaObject.transform.localPosition;

                position.x = player.maxMana / 100 + 2;

                maxManaObject.transform.localPosition = position;
            }
        }

    }
}
