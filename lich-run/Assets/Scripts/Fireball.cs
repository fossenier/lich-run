using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
