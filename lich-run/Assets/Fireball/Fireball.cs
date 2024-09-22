using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    void Start()
    {
        // Limit the fireball life to 5 seconds
        Destroy(gameObject, 5f);
    }
}