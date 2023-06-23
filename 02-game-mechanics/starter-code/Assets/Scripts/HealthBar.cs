using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HealthBar class manages the visual representation of an entity's health.
public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float originalScale;

    // Start is called before the first frame update
    void Start()
    {
        // Store the original scale of the health bar.
        originalScale = gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Adjust the scale of the health bar based on the current health.
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * originalScale;
        gameObject.transform.localScale = tmpScale;
    }
}
