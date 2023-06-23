// This file contains the Sheep class, which represents a sheep in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    // Private variables to store the collider, rigidbody, sheep spawner, hit by hay flag, hay destroy delay, run speed, heart offset, heart prefab, dropped flag, and drop destroy delay.
    private Collider myCollider;
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;
    private bool hitByHay;
    public float gotHayDestroyDelay;
    public float runSpeed;
    public float heartOffset;
    public GameObject heartPrefab;
    private bool dropped;
    public float dropDestroyDelay;

    // Start() method called when the Sheep object is created.
    void Start()
    {
        // Gets the collider and rigidbody components.
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();

        // Sets the sheep spawner.
        sheepSpawner = sheepSpawner;
    }

    // Update() method called every frame.
    void Update()
    {
        // Moves the sheep forward.
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }
    // // "Called when the sheep enters a trigger collider."
    private void OnTriggerEnter(Collider other)
    {
        // If the other collider has the "Hay" tag and the sheep has not been hit by hay yet, then the sheep is hit by hay.
        if (other.CompareTag("Hay") && !hitByHay)
        {
            // Destroys the hay bale.
            Destroy(other.gameObject);

            // Hits the sheep by setting the hit by hay flag to true, setting the run speed to 0, and destroying the sheep after a delay.
            hitByHay = true;
            runSpeed = 0;
            Destroy(gameObject, gotHayDestroyDelay);

            // Spawns a heart at the sheep's position.
            Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);

            // Tweens the sheep's scale to 0 over the gotHayDestroyDelay time.
            TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
            tweenScale.targetScale = 0;
            tweenScale.timeToReachTarget = gotHayDestroyDelay;

            // Plays the sheep hit sound.
            SoundManager.Instance.PlaySheepHitClip();
            GameStateManager.Instance.SavedSheep();
        }

        else if (other.CompareTag("DropSheep") && !dropped)
        {
            // Drops the sheep by setting the dropped flag to true, disabling the rigidbody and collider, and destroying the sheep after a delay.
            dropped = true;
            myRigidbody.isKinematic = false;
            myCollider.isTrigger = false;
            Destroy(gameObject, dropDestroyDelay);

            GameStateManager.Instance.DroppedSheep();

            SoundManager.Instance.PlaySheepDroppedClip();
        }
    }

    // // "Called when the sheep is hit by hay."
    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true;
        runSpeed = 0;

        Destroy(gameObject, gotHayDestroyDelay);
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);

        TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
        tweenScale.targetScale = 0; 
        tweenScale.timeToReachTarget = gotHayDestroyDelay;

        SoundManager.Instance.PlaySheepHitClip();
        GameStateManager.Instance.SavedSheep();
    }


    private void Drop()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        dropped = true;

        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
        Destroy(gameObject, dropDestroyDelay);

        GameStateManager.Instance.DroppedSheep();
        SoundManager.Instance.PlaySheepDroppedClip();
    }

    //Used by SheepSpawner.cs to spawn sheep in spawner locations
    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }
}

