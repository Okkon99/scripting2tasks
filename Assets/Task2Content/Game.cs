using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
