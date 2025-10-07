using UnityEngine;
using UnityEngine.Tilemaps;

public class Breakaway : MonoBehaviour
{
    private Tilemap tilemap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) 
        {
            Vector2 hitPosition = collision.contacts[0].point;
            
            // Convert world position to cell position
            Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);
            
            // Check if there's a tile at that position and remove it
            if (tilemap.HasTile(cellPosition))
            {
                tilemap.SetTile(cellPosition, null);
                
                // Optional: Add effects here
                // Instantiate(tileBreakEffect, tilemap.GetCellCenterWorld(cellPosition), Quaternion.identity);
            }
            
            // Destroy the projectile
            Destroy(collision.gameObject);
        }
    }
}
