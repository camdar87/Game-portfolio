using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameManagerBehaviour class manages the game state and UI elements.
public class GameManagerBehaviour : MonoBehaviour
{   
    public Text waveLabel;
    public GameObject[] nextWaveLabels;
    public bool gameOver = false;

    public Text healthLabel;
    public GameObject[] healthIndicator;

    public Text goldLabel;
    private int gold;
    public int Gold
    {
        // Property for accessing and updating the gold value.
        get { return gold; }
        set
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
        }
    }

    private int wave;
    public int Wave
    {
        // Property for accessing and updating the wave value.
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver)
            {
                // Trigger the next wave animation for all next wave labels.
                for (int i = 0; i < nextWaveLabels.Length; i++)
                {
                    nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
                }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }

    private int health;
    public int Health
    {
        // Property for accessing and updating the health value.
        get { return health; }
        set
        {
            // Shake the camera if the health is decreasing.
            if (value < health)
                Camera.main.GetComponent<CameraShake>().Shake();
            
            health = value;
            healthLabel.text = "HEALTH: " + health;

            // Check if the health is depleted and trigger the game over state.
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }

            // Activate or deactivate health indicators based on the health value.
            for (int i = 0; i < healthIndicator.Length; i++)
            {
                if (i < Health)
                    healthIndicator[i].SetActive(true);
                else
                    healthIndicator[i].SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize health, wave, and gold values.
        Health = 5;
        Wave = 0;
        Gold = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        // Update game logic here if necessary.
    }
}
