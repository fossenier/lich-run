using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform spawnPoint;
    public float fireballSpeed = 30f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SummonFireball();
        }
    }

    void SummonFireball()
    {
        if (fireballPrefab != null && spawnPoint != null)
        {
            // Instantiate the fireball and get its Rigidbody2D in one step
            GameObject newFireball = Instantiate(fireballPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody2D rb = newFireball.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = spawnPoint.right * fireballSpeed;
            }
            else
            {
                Debug.LogWarning("Fireball prefab does not have a Rigidbody2D component!");
            }
        }
        else
        {
            Debug.LogError("Fireball prefab or spawn point is not assigned!");
        }
    }
}
