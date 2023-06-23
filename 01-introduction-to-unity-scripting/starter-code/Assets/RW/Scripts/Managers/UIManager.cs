// This file contains the UIManager class, which handles updating the UI with the latest game state information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Static member variable to store the instance of the UIManager class.
    public static UIManager Instance;

    // Public Text variables to store the sheep saved and sheep dropped text.
    public Text sheepSavedText;
    public Text sheepDroppedText;

    // Public GameObject variable to store the game over window GameObject.
    public GameObject gameOverWindow;

    // Awake() method called when the UIManager object is created.
    void Awake()
    {
        // Set the static Instance variable to this object.
        Instance = this;
    }

    // UpdateSheepSaved() method updates the sheep saved text with the latest value from the GameStateManager class.
    // // "Updates the sheep saved text with the latest value from the GameStateManager class."
    public void UpdateSheepSaved()
    {
        sheepSavedText.text = GameStateManager.Instance.sheepSaved.ToString();
    }

    // UpdateSheepDropped() method updates the sheep dropped text with the latest value from the GameStateManager class.
    // // "Updates the sheep dropped text with the latest value from the GameStateManager class."
    public void UpdateSheepDropped()
    {
        sheepDroppedText.text = GameStateManager.Instance.sheepDropped.ToString();
    }

    // ShowGameOverWindow() method sets the game over window GameObject to active.
    // // "Sets the game over window GameObject to active."
    public void ShowGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }
}