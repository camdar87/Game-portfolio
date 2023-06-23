
//This script controls the game flow and initializes the maze, player, and monster. It communicates with the AIController and MazeConstructor scripts.
using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    private AIController aIController;
    private MazeConstructor constructor;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    [SerializeField] private int rows;
    [SerializeField] private int cols;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
        aIController = GetComponent<AIController>(); 
    }
    
    // Start is called before the first frame update.
    void Start()
    {
        constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);
        aIController.Graph = constructor.graph;
        aIController.Player = CreatePlayer();
        aIController.Monster = CreateMonster(); 
        aIController.HallWidth = constructor.hallWidth;         
        aIController.StartAI();
    }

    // Creates the player object and sets its initial position
    private GameObject CreatePlayer()
    {
        // Set the initial position of the player in the maze
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";

        return player;
    }

    // Creates the monster object and sets its position based on the maze goal
    private GameObject CreateMonster()
    {
        // Set the position of the monster based on the maze goal
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";    

        return monster;
    }

    // Handles the trigger event when the player reaches the treasure
    private void OnTreasureTrigger(GameObject trigger, GameObject other)
    { 
        Debug.Log("You Won! The monster is Gone");
        aIController.StopAI();
    }
}
