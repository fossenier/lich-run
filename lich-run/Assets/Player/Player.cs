using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator attackAnimator;
    public Rigidbody2D rigidBody;

    public float speed = 500f;
    public float jumpHeight = 5f;

    void Start()
    {
        attackAnimator = GetComponent<Animator>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("right"))
        {
            rigidBody.AddForce(transform.right * 500f * Time.deltaTime);
        }

        if (Input.GetKey("left"))
        {
            rigidBody.AddForce(transform.right * -500f * Time.deltaTime);
        }

        if (Input.GetKey("up") && transform.position.y <= -3.25)
        {
            rigidBody.AddForce(transform.up * 1000f * jumpHeight * Time.deltaTime);
        }

        //Attack Animation
        if (Input.GetKeyDown("s"))
        {
            attackAnimator.SetBool("Attack", true);
        }
        if (Input.GetKeyUp("s"))
        {
            attackAnimator.SetBool("Attack", false);
        }

    }

    void onAttack()
    {

    }
}
