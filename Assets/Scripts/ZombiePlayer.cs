using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;

    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    // --- NEW ---
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {

        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");


        moveDirection = moveDirection.normalized;

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

            if (moveDirection.sqrMagnitude > 0f)
                lastMoveDirection = moveDirection;

            animator.SetFloat("X", lastMoveDirection.x);
            animator.SetFloat("Y", lastMoveDirection.y);
        }

        if (spriteRenderer != null)
            spriteRenderer.flipX = lastMoveDirection.x < 0f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    // --- NEW: call this when the player touches a powerup ---

}
