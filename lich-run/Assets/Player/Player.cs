using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator attackAnimator;
    public Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite attackSprite;

    public float speed = 500f;
    public float jumpHeight = 5f;

    void Start()
    {
        attackAnimator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("right"))
        {
            rigidBody.AddForce(transform.right * 500f * Time.deltaTime);
            spriteRenderer.flipX = false; // Face right
        }

        if (Input.GetKey("left"))
        {
            rigidBody.AddForce(transform.right * -500f * Time.deltaTime);
            spriteRenderer.flipX = true; // Face left
        }

        if (Input.GetKey("up") && transform.position.y <= -3.25)
        {
            rigidBody.AddForce(transform.up * 1000f * jumpHeight * Time.deltaTime);
        }

        //Attack Animation
        if (Input.GetKeyDown("s"))
        {
            attackAnimator.SetBool("Attack", true);
            spriteRenderer.sprite = attackSprite;
        }
        if (Input.GetKeyUp("s"))
        {
            attackAnimator.SetBool("Attack", false);
        }

    }

    public void OnAnimationEventTriggered()
    {
        Debug.Log("Animation event triggered!");
    }

    void onAttack()
    {

    }

}
