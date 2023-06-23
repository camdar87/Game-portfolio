using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MoveEnemy class handles the movement and behavior of an enemy object.
public class MoveEnemy : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the last waypoint switch time.
        lastWaypointSwitchTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the start and end positions of the current waypoint.
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        // Calculate the length and time for the current path segment.
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;

        // Move the enemy object along the path segment using linear interpolation.
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        // Check if the enemy has reached the end of the current path segment.
        if (gameObject.transform.position.Equals(endPosition))
        {
            if (currentWaypoint < waypoints.Length - 2)
            {
                // Switch to the next waypoint and update the last waypoint switch time.
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                RotateIntoMoveDirection();
            }
            else
            {
                // Destroy the enemy, play audio, and update the game manager's health.
                Destroy(gameObject);
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                GameManagerBehaviour gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
                gameManager.Health -= 1;
            }
        }
    }

    // Rotate the enemy sprite to face the movement direction.
    private void RotateIntoMoveDirection()
    {
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);

        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;

        GameObject sprite = gameObject.transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    // Calculate the remaining distance to the goal from the current waypoint.
    public float DistanceToGoal()
    {
        float distance = Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }
}
