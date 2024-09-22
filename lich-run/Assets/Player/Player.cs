using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Animator attackAnimator;
    public Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite attackSprite;

    public float speed = 500f;
    public float jumpHeight = 5f;

    // Screen Bounds
    private Vector2 screenBounds;

    void Start()
    {
        // screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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

        // Vector3 playerPos = transform.position;
        // playerPos.x = Mathf.Clamp(playerPos.x, screenBounds.x, screenBounds.x * -1);
        // transform.position = playerPos;

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

        // Change Scenes
        if (Input.GetKeyDown("c"))
        {
            SceneManager.LoadScene("Lastest");
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
