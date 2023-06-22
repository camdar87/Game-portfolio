// MazeConstructor.cs
// 
// This script is responsible for generating and displaying a procedural maze in the scene.
// It uses the MazeMeshGenerator script to generate the mesh for the maze and handles the creation of game objects and materials.
// The maze is represented as a 2D integer array, where 0 represents an empty space and 1 represents a wall.

using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    public float placementThreshold = 0.1f;   // Chance of empty space
    private MazeMeshGenerator meshGenerator;

    public Node[,] graph;

    public float hallWidth { get; private set; }
    public int goalRow { get; private set; }
    public int goalCol { get; private set; }

    public int[,] data
    {
        get; private set;
    }

    // Generate a maze from the specified dimensions.
    // Returns the generated maze represented as a 2D integer array.
    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }
                else if (i % 2 == 0 && j % 2 == 0 && Random.value > placementThreshold)
                {
                    maze[i, j] = 1;

                    int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    maze[i + a, j + b] = 1;
                }
            }
        }
        return maze;
    }

    private void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        hallWidth = meshGenerator.width;
        // Default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    // Generate a new maze with the specified dimensions.
    // sizeRows: Number of rows in the maze.
    // sizeCols: Number of columns in the maze.
    // treasureCallback: Callback function to handle triggering of the treasure event.
    public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler treasureCallback)
    {
        DisposeOldMaze();

        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size like 15,17,19.");
        }

        data = FromDimensions(sizeRows, sizeCols);

        goalRow = data.GetUpperBound(0) - 1;
        goalCol = data.GetUpperBound(1) - 1;

        graph = new Node[sizeRows, sizeCols];

        for (int i = 0; i < sizeRows; i++)
        {
            for (int j = 0; j < sizeCols; j++)
            {
                graph[i, j] = data[i, j] == 0 ? new Node(i, j, true) : new Node(i, j, false);
            }
        }

        DisplayMaze();
        PlaceGoal(treasureCallback);
    }

    // Dispose of the old maze by destroying all generated game objects with the "Generated" tag.
    private void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    // Display the generated maze by creating game objects for the maze walls and floor.
    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }

    // Place the treasure goal in the maze and set up its properties and event handling.
    private void PlaceGoal(TriggerEventHandler treasureCallback)
    {
        GameObject treasure = GameObject.CreatePrimitive(PrimitiveType.Cube);
        treasure.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
        treasure.name = "Treasure";
        treasure.tag = "Generated";

        treasure.GetComponent<BoxCollider>().isTrigger = true;
        treasure.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

        TriggerEventRouter tc = treasure.AddComponent<TriggerEventRouter>();
        tc.callback = treasureCallback;
    }

    // Display the maze as ASCII art in the GUI for debugging purposes.
    private void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                msg += maze[i, j] == 0 ? "...." : "==";
            }
            msg += "\n";
        }

        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
}
