using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlaceMonster class handles the placement and upgrading of a monster object.
public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject monster;

    private GameManagerBehaviour gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find and store the GameManagerBehaviour component from the GameManager object.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the mouse button is released over the object's collider
    void OnMouseUp()
    {
        if (CanPlaceMonster())
        {
            // Instantiate the monster prefab at the current position.
            monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            // Reduce the player's gold by the cost of the placed monster.
            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
        }
        else if (CanUpgradeMonster())
        {
            // Increase the level of the existing monster and play an upgrade audio.
            monster.GetComponent<MonsterData>().IncreaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            // Reduce the player's gold by the cost of the monster upgrade.
            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
        }

    }

    // Check if a monster can be placed based on the available gold and if no monster is currently placed.
    private bool CanPlaceMonster()
    {
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }

    // Check if the existing monster can be upgraded based on the available gold and the next level's cost.
    private bool CanUpgradeMonster()
    {
        if (monster != null)
        {
            MonsterData monsterData = monster.GetComponent<MonsterData>();
            MonsterLevel nextLevel = monsterData.GetNextLevel();
            if (nextLevel != null)
            {
                return gameManager.Gold >= nextLevel.cost;
            }
        }
        return false;
    }
}
