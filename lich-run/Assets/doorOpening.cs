using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpening : MonoBehaviour
{
    void Update()
    {
        // Red door, penalize them and let them through
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q key was pressed.");
        }

        // Green door, let them through
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed.");
        }
    }
}
