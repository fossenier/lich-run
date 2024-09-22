using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform spawnPoint;
    public float fireballSpeed = 30f;

    private SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer (where facing)

    void Start() {
        playerSpriteRenderer = GetComponentInParent<SpriteRenderer>(); // Get the SpriteRenderer component from the parent object
        if (playerSpriteRenderer == null) {
            Debug.LogError("Player SpriteRenderer not found!");
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SummonFireball();
        }
    }

    void SummonFireball() {
        if (fireballPrefab != null && spawnPoint != null && playerSpriteRenderer != null) {
            
            // Instantiate the fireball and get its Rigidbody2D
            GameObject newFireball = Instantiate(fireballPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody2D rb = newFireball.GetComponent<Rigidbody2D>();
            
            // Determine the direction the player is facing
            Vector2 direction = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;
            
            // Get the fireball's SpriteRenderer
            SpriteRenderer fireballSprite = newFireball.GetComponent<SpriteRenderer>(); 
            if (fireballSprite != null) {
                fireballSprite.flipX = (direction == Vector2.left);
            } else {
                Debug.LogWarning("Fireball prefab does not have a SpriteRenderer component!");
            }

            if (rb != null) {
                rb.velocity = direction * fireballSpeed;
            } else {
                Debug.LogWarning("Fireball prefab does not have a Rigidbody2D component!");
            }


        } else {
            Debug.LogError("Fireball prefab or spawn point is not assigned!");
        }
    }
}
