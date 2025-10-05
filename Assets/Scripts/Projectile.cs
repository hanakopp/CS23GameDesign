using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 25;
    public float lifetime = 3f; // destroy if it flies too long

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if it hit an enemy
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
                Debug.Log("bullet hit enemy!");
            enemy.TakeDamage(damage);
        }

        // Destroy the projectile regardless 
        Destroy(gameObject);
    }

    // If you’re using triggers instead of colliders, use this instead:
    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMelee enemy = collision.GetComponent<EnemyMelee>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    */
}