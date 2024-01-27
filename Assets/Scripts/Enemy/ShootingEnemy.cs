using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    //This script controls enemy shooting mechanics, available projectiles and projectile to use

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootingPoint;

    CameraMovement cameraMovement;

    [Header("Prohectiles")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] List<GameObject> projectilesAvailable = new List<GameObject>();

    [SerializeField] AudioSource shootSound;
    [SerializeField] Animator spritesAnimator;
    void Start()
    {
        AddProjectileToList(0); //add default projectile to list
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    public void FireProjectile(Vector2 velocity)
    {
        spritesAnimator.SetTrigger("shoot");
        shootSound.Play();
        SetRandomProjectile();
        GameObject instanceProjectile = Instantiate(projectilePrefab, shootingPoint.position,
                shootingPoint.rotation);
        instanceProjectile.GetComponent<Rigidbody2D>().velocity = velocity;

        cameraMovement.SetNewProjetileTarget(instanceProjectile.transform);        
    }

    private void SetRandomProjectile()
    {
        //Set a random projectile from the ones available
        int randomIndexProjectile = Random.Range(0, projectilesAvailable.Count);
        projectilePrefab = projectilesAvailable[randomIndexProjectile];
    }

    public void AddProjectileToList(int index)
    {
        //add a new projectile to the list
        if (!projectilesAvailable.Contains(projectiles[index]))
        {
            projectilesAvailable.Add(projectiles[index]);
        }
    }
}
