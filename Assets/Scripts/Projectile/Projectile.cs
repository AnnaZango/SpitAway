using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    //This script is attached to any projectiles prefab, which is instantiated when the enemy or
    //player shoot. 

    Rigidbody2D rb;
    ProjectileProperties projectileProperties;
    EnemyAI enemy;
    PlayerStats player;
    LayerMask impactLayerMask;
    CameraMovement cameraMovement;
    TurnController turnController;
    GameObject vfx;

    Collider2D[] colliders;

    Tilemap tilemapDestructible;

    bool hasHurt = false;
    bool vfxInstantiated = false;

    AudioSource explosionSound;
    
    void Awake()
    {
        projectileProperties = GetComponent<ProjectileProperties>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();

        enemy = FindObjectOfType<EnemyAI>();
        player = FindObjectOfType<PlayerStats>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        turnController = FindObjectOfType<TurnController>();
        tilemapDestructible = GameObject.FindWithTag("Destructible").GetComponent<Tilemap>();

        explosionSound = GameObject.FindGameObjectWithTag("explosionSound").GetComponent<AudioSource>();
    }

    private void Start()
    {
        impactLayerMask = (1 << LayerMask.NameToLayer("Default"))
                     | (1 << LayerMask.NameToLayer("Player"))
                     | (1 << LayerMask.NameToLayer("Enemy"));
        
        //Destroy projectile if it didn't hit anything and lifetime is over 
        Invoke(nameof(DestroyProjectile), projectileProperties.GetTimeAlive());        

        cameraMovement.SetCharacterTargetNull(); //no longer following a player
    }

    private void Update()
    {
        Vector2 velocity = rb.velocity;
        float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        List<GameObject> objectsHit = rb.RaycastAll(Vector2.down, projectileProperties.GetDamageRadius(), 0, impactLayerMask);

        //We enable all colliders to detect surrounding area and destroy tiles (after some delay
        //depending on particle effects)

        Invoke(nameof(EnableAllColliders), projectileProperties.GetTimeParticles());

        if (!hasHurt)
        {
            for (int i = 0; i < objectsHit.Count; i++)
            {
                hasHurt = true;
                if (objectsHit[i].tag == "Enemy")
                {
                    enemy.ChangeHealthPoints(-projectileProperties.GetDamagePoints());
                }
                else if (objectsHit[i].tag == "Player")
                {
                    player.ChangeHealthPoints(-projectileProperties.GetDamagePoints());
                }
            }
        }        

        if (transform.GetChild(0).gameObject != null) { transform.GetChild(0).gameObject.SetActive(false); }

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(!vfxInstantiated)
        {
            vfx = Instantiate(projectileProperties.GetVFX(), transform.position, Quaternion.identity);
            vfxInstantiated = true;
            explosionSound.Play();
        }
         
        Invoke(nameof(DestroyProjectile), 1f); //slight delay to destroy projectile
    }

    private void EnableAllColliders()
    {
        //we need at least 2 circle colliders (one smaller, one larger) to get the full range,
        //otherwise tiles in the middle ignored and not destroyed
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        Invoke(nameof(DisableAllColliders), 0.5f);
    }

    private void DisableAllColliders()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void SetEnemyAsCamTarget()
    {
        cameraMovement.SetEnemyTarget();
    } 
    private void SetPlayerAsCamTarget()
    {
        cameraMovement.SetPlayerTarget();
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        // Draw a yellow sphere at the transform's position, for debugging purposes, to see range
        Gizmos.color = new Color32(0, 255, 255, 100);
        Gizmos.DrawSphere(transform.position, projectileProperties.GetDamageRadius());
    }

    public void Initialize(float power)
    {
        if (rb != null)
        {
            rb.AddForce(transform.right * power, ForceMode2D.Impulse);
        }
    }

    private void DestroyProjectile()
    {      
        //Before destroying, we assign the new camera target so the camera goes there

        if (turnController.GetIfPlayerTurn())
        {
            Invoke(nameof(SetEnemyAsCamTarget), 2);
        }
        else
        {
            Invoke(nameof(SetPlayerAsCamTarget), 2);
        }
        
        GetComponent<BoxCollider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Destroy(vfx, 2f);
        Destroy(gameObject, 3f);        
    }

}
