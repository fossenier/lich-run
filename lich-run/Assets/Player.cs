using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform sword;
    public Rigidbody2D rigidBody;

    private bool isSwordExtended = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(transform.position.y);
        if (Input.GetKey("right"))
        {
            rigidBody.AddForce(transform.right * 500f * Time.deltaTime);
        }

        if (Input.GetKey("left"))
        {
            rigidBody.AddForce(transform.right * -500f * Time.deltaTime);
        }

        if (Input.GetKey("up") && transform.position.y <= -3.4)
        {
            rigidBody.AddForce(transform.up * 5000f * Time.deltaTime);
        }

        if (Input.GetKey("s"))
        {
            // rigidBody.AddForce(transform.up * 5000f * Time.deltaTime);
            if (!isSwordExtended && sword.rotation.z == 0)
            {
                sword.Rotate(0, 0, -90);
                isSwordExtended = true;
                // yield return new WaitForSeconds(1);
                sword.Rotate(0, 0, 90);

            }
        }

    }
}
