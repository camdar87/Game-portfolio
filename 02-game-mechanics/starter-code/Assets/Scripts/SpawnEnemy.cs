using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave class represents a wave of enemies with spawn settings.
[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2;
    public int maxEnemies = 20;
}

// SpawnEnemy class handles the spawning of enemies based on wave settings.
public class SpawnEnemy : MonoBehaviour
{
    public Wave[] waves;
    public int timeBetweenWaves = 5;

    private GameManagerBehaviour gameManager;

    private float lastSpawnTime;
    private int enemiesSpawned = 0;

    public GameObject[] waypoints;
    public GameObject testEnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check the current wave and spawn enemies accordingly.
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;

            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                (enemiesSpawned != 0 && timeInterval > spawnInterval)) &&
                (enemiesSpawned < waves[currentWave].maxEnemies))
            {
                // Spawn a new enemy.
                lastSpawnTime = Time.time;
                GameObject newEnemy = (GameObject)Instantiate(waves[currentWave].enemyPrefab);
                newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                enemiesSpawned++;
            }

            if (enemiesSpawned == waves[currentWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                // Increase the wave count, update gold, and reset spawn variables.
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            // The game is over, show game won animation.
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
}
