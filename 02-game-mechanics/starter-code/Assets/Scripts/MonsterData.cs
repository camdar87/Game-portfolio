using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonsterLevel class represents a specific level of a monster.
[System.Serializable]
public class MonsterLevel
{
    public int cost;
    public GameObject visualization;
    public GameObject bullet;
    public float fireRate;
}

// MonsterData class manages the data and behavior of a monster.
public class MonsterData : MonoBehaviour
{
    public List<MonsterLevel> levels;
    private MonsterLevel currentLevel;

    public MonsterLevel CurrentLevel
    {
        // Property for accessing and updating the current monster level.
        get { return currentLevel; }
        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);
            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            
            // Activate the visualization for the current level and deactivate others.
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                        levels[i].visualization.SetActive(true);
                    else
                        levels[i].visualization.SetActive(false);
                }
            }
        }
    }

    void OnEnable()
    {
        // Set the current level to the first level when the object is enabled.
        CurrentLevel = levels[0];
    }

    // Get the next level of the monster, returns null if it is already at the max level.
    public MonsterLevel GetNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

    // Increase the level of the monster if it is not already at the max level.
    public void IncreaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }
}
