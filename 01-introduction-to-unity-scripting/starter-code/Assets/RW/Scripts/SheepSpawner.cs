// This class is a sheep spawner. It spawns sheep at random positions
// and adds them to a list.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    // The sheep prefab to spawn.
    [SerializeField]
    private GameObject sheepPrefab;

    public bool canSpawn = true;

    public float timeBetweenSpawns = 1.0f;

    public List<Transform> sheepSpawnPositions = new List<Transform>();

    // A list of the sheep that have been spawned.
    private List<GameObject> sheepList = new List<GameObject>();

    // Start the spawner.
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Spawn a sheep.
    private void SpawnSheep()
    {
        Vector3 randomPosition = sheepSpawnPositions[Random.Range(0, sheepSpawnPositions.Count)].position;
        GameObject sheep = Instantiate(sheepPrefab, randomPosition, sheepPrefab.transform.rotation);
        sheepList.Add(sheep);
        sheep.GetComponent<Sheep>().SetSpawner(this);
    }

    // A coroutine that spawns sheep at regular intervals.
    private IEnumerator SpawnRoutine()
    {
 
        while (canSpawn)
        {
            // Spawn a sheep.
            SpawnSheep();

 
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    // Remove a sheep from the list of sheep.
    public void RemoveSheepFromList(GameObject sheep)
    {
        // Remove the sheep from the list.
        sheepList.Remove(sheep);
    }


    public void DestroyAllSheep()
    {
        // For each sheep in the list, destroy it.
        foreach (GameObject sheep in sheepList)
        {
            Destroy(sheep);
        }

        // Clear the list.
        sheepList.Clear();
    }
}