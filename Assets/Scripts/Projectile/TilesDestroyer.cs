using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesDestroyer : MonoBehaviour
{
    // This script is attached to the projectile and is responsible for destroying the tiles that
    // are within range of the collision point


    public Tilemap tilemap;
    public ContactPoint2D[] contacts;
    CircleCollider2D colliderCircle;

    private void Awake()
    {
        tilemap = GameObject.FindWithTag("Destructible").GetComponent<Tilemap>();        
    }
    
    void Start()
    {
        colliderCircle = GetComponent<CircleCollider2D>();
        colliderCircle.radius = GetComponent<ProjectileProperties>().GetDamageRadius();        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {        
        //we get all contact points from the collision, and destroy the tiles at that position

        Vector3 hitPosition = Vector3.zero;

        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x; 
            hitPosition.y = hit.point.y; 
           
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }

}
