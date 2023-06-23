// This file contains the GameStateManager class, which handles tracking the game state.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    // Static member variable to store the instance of the GameStateManager class.
    public static GameStateManager Instance;

    [HideInInspector]
    public int sheepSaved;
    
    [HideInInspector]
    public int sheepDropped;

    public int sheepDroppedBeforeGameOver; 
    public SheepSpawner sheepSpawner;

    // Awake() method called when the GameStateManager object is created.
    void Awake()
    {
        Instance = this;
    }

    // Update() method called every frame.
    void Update()
    {
        // If the Escape key is pressed, load the Title scene.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    // SavedSheep() method increments the sheep saved counter and updates the UI.
    public void SavedSheep()
    {
        sheepSaved++;
        UIManager.Instance.UpdateSheepSaved();
    }

    // DroppedSheep() method increments the sheep dropped counter and updates the UI, and then checks if the game is over.
    public void DroppedSheep()
    {
        sheepDropped++;
        UIManager.Instance.UpdateSheepDropped();

        if (sheepDropped == sheepDroppedBeforeGameOver)
        {
            GameOver();
        }
    }

    // GameOver() method sets the sheep spawner to not spawn sheep, destroys all sheep, and shows the game over window.
    private void GameOver()
    {
        sheepSpawner.canSpawn = false;
        sheepSpawner.DestroyAllSheep();
        UIManager.Instance.ShowGameOverWindow();
    }
}