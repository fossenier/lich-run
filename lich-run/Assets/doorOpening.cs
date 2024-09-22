using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpening : MonoBehaviour
{
    // This needs to be hooked up to the clock and Scene Manager
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q key was pressed.");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed.");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key was pressed.");
        }
    }
}
