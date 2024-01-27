using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingPlayer : MonoBehaviour
{
    // This script is responsible for the available projectiles and the actual firing of the
    // projectile.

    [SerializeField] GameObject currentProjectilePrefab;
    [SerializeField] GameObject[] allProjectilePrefabs;
    [SerializeField] Button[] buttonsProjectiles;
    [SerializeField] Transform shootingPoint;

    CameraMovement cameraMovement;

    [SerializeField] AudioSource shootSound;

    private void Start()
    {
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
        for (int i = 0; i < buttonsProjectiles.Length; i++)
        {
            //at start, only the default projectile enabled, and colors highlighted to show that
            if (i == 0)
            {
                buttonsProjectiles[i].image.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                //transparent color for disabled projectile buttons
                buttonsProjectiles[i].image.color = new Color32(30, 30, 30, 70);
                buttonsProjectiles[i].enabled = false;
            }            
        }
    }

    public void FireProjectile(float power)
    {
        //the projectile is instantiated and the target of the camera is set to it

        shootSound.Play();
        GameObject instanceProjectile = Instantiate(currentProjectilePrefab, shootingPoint.position, 
            shootingPoint.rotation);
        if (transform.localScale.x < 0)
        {
            //if player looking to the left
            instanceProjectile.GetComponent<Projectile>().Initialize(-power);
        }
        else
        {
            //if player looking to the right
            instanceProjectile.GetComponent<Projectile>().Initialize(power);
        }        
        
        cameraMovement.SetNewProjetileTarget(instanceProjectile.transform);
    }

    
    public void AssignProjectile(int index) //called when pressing projectile buttons, 
                                            //to assign current project
    {

        if (index < allProjectilePrefabs.Length)
        {
            currentProjectilePrefab = allProjectilePrefabs[index];
        }
        else
        {
            Debug.Log("Projectile index outside range!");
            currentProjectilePrefab = allProjectilePrefabs[0];
        }

        // the button selected is highlighted, the others grey
        for (int i = 0; i < buttonsProjectiles.Length; i++)
        {
            if(i == index)
            {
                buttonsProjectiles[i].image.color = new Color32(255, 255, 255, 255);                
            }
            else if (buttonsProjectiles[i].enabled)
            {
                buttonsProjectiles[i].image.color = new Color32(100, 100, 100, 255);            
            }
        }
    }

    
    public void EnableProjectileButton(int index)
    {
        //This method is called when the player picks up a projectile pickup 

        buttonsProjectiles[index].image.color = new Color32(255, 255, 255, 255);
        buttonsProjectiles[index].enabled = true;
        buttonsProjectiles[index].image.color = new Color32(100, 100, 100, 255);        
    }
    
}
