using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Friendly : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb2D;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float activationRange = 10f;
    public float pointReachThreshold = 0.2f;

    [Header("References")]
    public Transform player;
    public Transform[] moveSpots; // multiple destinations

    private bool isReleased = false;
    private int currentSpotIndex = 0;
    private float scaleX;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Optionally disable patrol script (if this NPC used to have one)
        NPC_PatrolSequencePoints patrol = GetComponent<NPC_PatrolSequencePoints>();
        if (patrol != null)
            patrol.enabled = false;
    }

    void Update()
    {
        if (player == null || moveSpots.Length == 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Activate NPC when player is close enough
        if (!isReleased && distanceToPlayer <= activationRange)
        {
            isReleased = true;
            //anim.SetBool("Walk", true);
        }

        if (isReleased)
            MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (currentSpotIndex >= moveSpots.Length)
        {
            // Reached final point — stop movement
            //anim.SetBool("Walk", false);
            Destroy(gameObject);
            return;
        }

        Transform currentTarget = moveSpots[currentSpotIndex];
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Face movement direction
        if (currentTarget.position.x > transform.position.x)
            transform.localScale = new Vector2(Mathf.Abs(scaleX), transform.localScale.y);
        else
            transform.localScale = new Vector2(-Mathf.Abs(scaleX), transform.localScale.y);

        // Check if reached this point
        if (Vector2.Distance(transform.position, currentTarget.position) < pointReachThreshold)
        {
            currentSpotIndex++; // move to next point
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRange);

        // Draw lines between waypoints (optional visual aid)
        if (moveSpots != null && moveSpots.Length > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < moveSpots.Length - 1; i++)
            {
                if (moveSpots[i] && moveSpots[i + 1])
                    Gizmos.DrawLine(moveSpots[i].position, moveSpots[i + 1].position);
            }
        }
    }
}
