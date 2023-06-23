// this is used for moving very simple.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 movementSpeed;
    public Space space;

    // Updates position of gameobject over time
    void Update()
    {
        transform.Translate(movementSpeed * Time.deltaTime, space);
    }
}

