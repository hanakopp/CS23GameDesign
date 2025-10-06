using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;

    public GameObject playerArt;

    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private bool facingRight;

    public Transform FireBase;

    public Vector2 momentum;
    public Vector2 direction;

    private Vector3 ProjDirection;

    public Sprite UpSprite;
    public Sprite DownSprite;
    public Sprite SideSprite;

    public GameObject projpre;

    private Rigidbody2D projmass;


    // --- NEW ---
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = playerArt.GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
        FireBase.position = Vector2.zero;
    }

    void Update()
    {

        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");


        momentum = (moveDirection).normalized;


        moveDirection = moveDirection.normalized;



        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

            if (moveDirection.sqrMagnitude > 0f)
                lastMoveDirection = moveDirection;

            animator.SetFloat("X", lastMoveDirection.x);
            animator.SetFloat("Y", lastMoveDirection.y);
        }

        if (spriteRenderer != null && moveDirection.y > 0f)
        {
            spriteRenderer.sprite = UpSprite;
            direction = Vector2.up;

        }
        if (spriteRenderer != null && moveDirection.y < 0f)
        {
            spriteRenderer.sprite = DownSprite;
            direction = Vector2.down;
        }


        if (spriteRenderer != null && moveDirection.x != 0f)
        {
            spriteRenderer.sprite = SideSprite;
            spriteRenderer.flipX = moveDirection.x < 0f;
            if (spriteRenderer.flipX) { direction = Vector2.left; }
            else { direction = Vector2.right ; }
        }

        if (moveDirection != Vector2.zero) {
            Vector2 halfway = 0.5f * (direction).normalized;
            FireBase.localPosition = halfway;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayerFire();
        }
        

        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;

        // FireBase.localPosition = momentum;

        // if (spriteRenderer.sprite == DownSprite) {
        //     FireBase.localPosition = Vector2.down;
        // }
        // else if (spriteRenderer.sprite == UpSprite) {
        //     FireBase.localPosition = Vector2.up;
        // } else if (spriteRenderer.sprite == SideSprite ) {
        //     if (spriteRenderer.flipX) { FireBase.localPosition = Vector2.left; }
        //     else { FireBase.localPosition = Vector2.right; }
        // }
    }

    // --- NEW: flips the sprite upon moving left/right
    // void Flip() 
    // {
    //     facingRight = !facingRight;
    //     Vector3 thescale = transform.localScale;
    //     theScale.x *= -1;
    //     transform.localScale = theScale;
    // }

    // --- NEW: call this when the player touches a powerup ---

    void PlayerFire()
    {
        GameObject projectile = Instantiate(projpre, FireBase, false);
        projectile.transform.parent = null;
        projmass = projectile.GetComponent<Rigidbody2D>();
        Vector2 force = 50 * direction;
        projmass.AddForce(force, ForceMode2D.Impulse);

    }

}
