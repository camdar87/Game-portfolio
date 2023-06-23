// This file contains the HayMachine class, which represents a hay machine in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMachine : MonoBehaviour
{
    // Public variables to store the model parent, model prefabs, hay spawnpoint, hay bale prefab, movement speed, horizontal boundary, shoot interval, and shoot timer.
    public Transform modelParent;
    public GameObject blueModelPrefab;
    public GameObject yellowModelPrefab;
    public GameObject redModelPrefab;
    public Transform haySpawnpoint;
    public GameObject hayBalePrefab;
    public float movementSpeed;
    public float horizontalBoundary = 22; 
    public float shootInterval;
    private float shootTimer;

    // Start() method called when the HayMachine object is created.
    void Start()
    {
        LoadModel();
    }

    // Update() method called every frame.
    void Update()
    {
        // Updates the movement and shooting.
        UpdateMovement();
        UpdateShooting();
    }

    // // "Loads the model based on the game settings."
    private void LoadModel()
    {
        // Destroys the existing model.
        Destroy(modelParent.GetChild(0).gameObject);

        // Loads the model based on the game settings.
        switch (GameSettings.hayMachineColor)
        {
            case HayMachineColor.Blue:
                Instantiate(blueModelPrefab, modelParent);
                break;

            case HayMachineColor.Yellow:
                Instantiate(yellowModelPrefab, modelParent);
                break;

            case HayMachineColor.Red:
                Instantiate(redModelPrefab, modelParent);
                break;
        }
    }

    // // "Updates the movement of the hay machine."
    private void UpdateMovement()
    {
        // Gets the horizontal input from the player.
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Moves the hay machine left or right if the input is within the boundary.
        if (horizontalInput < 0 && transform.position.x > -horizontalBoundary)
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0 && transform.position.x < horizontalBoundary)
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    }

    // // "Updates the shooting of the hay machine."
    private void UpdateShooting()
    {
        // Decrements the shoot timer.
        shootTimer -= Time.deltaTime;

        // Shoots hay if the shoot timer is <= 0 and the spacebar is pressed.
        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space))
        {
            shootTimer = shootInterval;
            ShootHay();
        }
    }

    // // "Shoots hay."
    private void ShootHay()
    {
        // Instantiates a hay bale at the hay spawnpoint.
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);

        // Plays the shoot sound.
        SoundManager.Instance.PlayShootClip();
    }
}