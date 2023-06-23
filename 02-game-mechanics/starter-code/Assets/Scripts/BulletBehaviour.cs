using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BulletBehaviour class represents the behavior of a bullet fired by the player.
public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10;
    public int damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private Vector3 normalizeDirection;
    private GameManagerBehaviour gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the normalized direction from the start position to the target position.
        normalizeDirection = (targetPosition - startPosition).normalized;

        // Find and reference the GameManagerBehaviour component.
        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet towards the target based on the normalized direction, speed, and time.
        transform.position += normalizeDirection * speed * Time.deltaTime; 
    }

    // OnTriggerEnter2D is called when the bullet collides with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Assign the collided game object to the target variable.
        target = other.gameObject;

        // Check if the collided object is an enemy.
        if(target.tag.Equals("Enemy"))
        {
            // Find the health bar associated with the enemy and decrease its current health by the bullet's damage.
            Transform healthBarTransform = target.transform.Find("HealthBar");
            HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
            healthBar.currentHealth -= damage;

            // Check if the enemy's health is depleted.
            if (healthBar.currentHealth <= 0)
            {
                // Destroy the enemy game object, play its audio clip at the bullet's position, and increase the player's gold.
                Destroy(target);
                AudioSource audioSource = target.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                gameManager.Gold += 50;
            }

            // Destroy the bullet game object.
            Destroy(gameObject);
        }
    }
}
