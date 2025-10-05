using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        ZombiePlayer player = other.GetComponent<ZombiePlayer>();
        if (player != null)
        {
            player.UnlockVerticalMovement();
            Destroy(gameObject); // remove the powerup
        }
    }
}