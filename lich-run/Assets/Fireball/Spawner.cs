using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject fireballPrefab; // Reference to the fireball prefab
    public Transform spawnPoint; // Spawn point for the fireball
    public float fireballSpeed = 10f; // Speed of the fireball
    public int fireballLayer = 6; // Fireball layer (MonsterLayer)

    private SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer

    void Start()
    {
        // Get the SpriteRenderer component from the parent (the player)
        playerSpriteRenderer = GetComponentInParent<SpriteRenderer>();

        // Ignore collision between Lich (Layer 3) and Fireball (Layer 6)
        Physics2D.IgnoreLayerCollision(3, fireballLayer, true);
    }

    void Update()
    {
        // If spacebar is pressed, summon a fireball
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SummonFireball();
        }
    }

    void SummonFireball()
    {
        // Check if the necessary components are assigned
        if (fireballPrefab != null && spawnPoint != null && playerSpriteRenderer != null)
        {
            // Determine the direction based on player’s facing direction (flipX)
            Vector2 direction = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;

            // Calculate the offset based on the direction the player is facing
            Vector3 spawnOffset = Vector3.zero;

            // These correct for the Lich's player size
            if (direction == Vector2.right)
            {
                spawnOffset = new Vector3(-0.25f, 0f, 0f);
            }
            else if (direction == Vector2.left)
            {
                spawnOffset = new Vector3(-2f, 0f, 0f);
            }

            // Apply the offset to the spawn point's position
            Vector3 spawnPosition = spawnPoint.position + spawnOffset;

            // Instantiate the fireball at the modified spawn position and spawnPoint's rotation
            GameObject newFireball = Instantiate(fireballPrefab, spawnPosition, spawnPoint.rotation);

            // Set the fireball to the correct layer (Fireball layer)
            newFireball.layer = fireballLayer;
            Debug.Log("Fireball Layer after spawn: " + newFireball.layer);

            // Get the Rigidbody2D component of the fireball
            Rigidbody2D rb = newFireball.GetComponent<Rigidbody2D>();

            // Flip the fireball sprite to match the direction
            SpriteRenderer fireballSprite = newFireball.GetComponent<SpriteRenderer>();
            if (fireballSprite != null)
            {
                fireballSprite.flipX = (direction == Vector2.left);
            }
            else
            {
                Debug.LogWarning("Fireball prefab does not have a SpriteRenderer component!");
            }

            // Apply velocity to the fireball
            if (rb != null)
            {
                rb.velocity = direction * fireballSpeed;
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