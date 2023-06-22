using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Constants for movement costs
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 140;

    // The graph representing the game world
    private Node[,] graph;
    public Node[,] Graph 
    {
        get { return graph; }
        set { graph = value; }
    }

    // The monster controlled by the AI
    private GameObject monster;
    public GameObject Monster 
    {
        get { return monster; }
        set { monster = value; }       
    }

    // The player object
    private GameObject player;
    public GameObject Player
    {
        get { return player; }
        set { player = value; } 
    }

    // The width of the hallways in the game world
    private float hallWidth;
    public float HallWidth 
    {
        get { return hallWidth; }
        set { hallWidth = value; }
    }

    // The speed of the monster
    [SerializeField] private float monsterSpeed;

    // Variables for tracking the starting position of the monster
    private int startRow = -1;
    private int startCol = -1;

    // Method for initializing the AI
    public void StartAI()
    {
        // Set the starting position of the monster to the upper right corner of the graph
        startRow = graph.GetUpperBound(0) - 1;
        startCol = graph.GetUpperBound(1) - 1;            
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the AI has been initialized
        if(startRow != -1 && startCol != -1)
        {            
            // Get the current position of the player
            int playerCol = (int)Mathf.Round(player.transform.position.x / hallWidth);
            int playerRow = (int)Mathf.Round(player.transform.position.z / hallWidth);
            
            // Find a path from the monster's current position to the player's position
            List<Node> path = FindPath(startRow, startCol, playerRow, playerCol);

            // Check if a valid path was found
            if(path != null && path.Count > 1)
            {
                // Get the next node in the path
                Node nextNode = path[1];
                float nextX = nextNode.y * hallWidth;
                float nextZ = nextNode.x * hallWidth;

                // Calculate the end position for the monster's movement
                Vector3 endPosition = new Vector3(nextX, 0f, nextZ);

                // Move the monster towards the end position
                float step = monsterSpeed * Time.deltaTime;
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, endPosition, step);

                // Calculate the new direction for the monster to face
                Vector3 targetDirection = endPosition - monster.transform.position;
                Vector3 newDirection = Vector3.RotateTowards(monster.transform.forward, targetDirection, step, 0.0f);
                monster.transform.rotation = Quaternion.LookRotation(newDirection);

                // Check if the monster has reached the end position
                if(monster.transform.position == endPosition){
                    // Update the starting position of the monster
                    startRow = nextNode.x;
                    startCol = nextNode.y;
                }
            }
        }
    }

    // Method for calculating the distance cost between two nodes
    private int CalculateDistanceCost(Node a, Node b){
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = xDistance - yDistance;
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // Method for finding the node with the lowest f cost in a list of nodes
    private Node GetLowestFCostNode(List<Node> pathNodeList){
        Node lowestFCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++)
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathNodeList[i];
                        
        return lowestFCostNode;
    }

    // Method for getting the list of neighboring nodes for a given node
    private List<Node> GetNeighbourList(Node currentNode){
        List<Node> neighbourList = new List<Node>();

        if(currentNode.x - 1 >= 0)
        {
            neighbourList.Add(graph[currentNode.x - 1,currentNode.y]);

            if(currentNode.y - 1 >= 0)
                neighbourList.Add(graph[currentNode.x - 1, currentNode.y - 1]);
            if(currentNode.y + 1 < graph.GetLength(1))
                neighbourList.Add(graph[currentNode.x - 1, currentNode.y + 1]);
        }

        if(currentNode.x + 1 < graph.GetLength(0))
        {
            neighbourList.Add(graph[currentNode.x + 1, currentNode.y]);
                
            if(currentNode.y - 1 >= 0) 
                neighbourList.Add(graph[currentNode.x + 1, currentNode.y - 1]);
            if(currentNode.y + 1 < graph.GetLength(1)) 
                neighbourList.Add(graph[currentNode.x + 1, currentNode.y + 1]);
        }

        if(currentNode.y - 1 >= 0) 
            neighbourList.Add(graph[currentNode.x, currentNode.y - 1]);
        if(currentNode.y + 1 < graph.GetLength(1)) 
            neighbourList.Add(graph[currentNode.x, currentNode.y + 1]);
            
        return neighbourList;
    }

    // Method for calculating the path from the start node to the end node
    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;

        // Backtrack from the end node to the start node to build the path
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        // Reverse the path to get it in the correct order
        path.Reverse();
        return path;
    }

    // Method for finding a path from the start position to the end position
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = graph[startX,startY];
        Node endNode = graph[endX, endY];

        List<Node> openList = new List<Node> { startNode };
        List<Node> closedList = new List<Node>();

        int graphWidth = graph.GetLength(0);
        int graphHeight = graph.GetLength(1);

        // Reset the gCost, hCost, and cameFromNode values for all nodes
        for(int x = 0; x < graphWidth; x++)
            for(int y = 0; y < graphHeight; y++)
            {
                Node pathNode = graph[x, y];
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            // Get the node with the lowest f cost from the open list
            Node currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)            
                return CalculatePath(endNode);            

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(Node neighbourNode in GetNeighbourList(currentNode)){
                if(closedList.Contains(neighbourNode)) continue;

                if(!neighbourNode.isWalkable){
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost){
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);  
                    neighbourNode.CalculateFCost();                  

                    if(!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }

        // Out of nodes on the open list, no path found
        return null;
    }
    
    // Method for stopping the AI and resetting the starting position
    public void StopAI()
    {
        startRow = -1;
        startCol = -1;
        Destroy(monster);
    }
}
