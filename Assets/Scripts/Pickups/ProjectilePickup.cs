using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : MonoBehaviour
{
    //Script assigned to projectile pickup prefab

    [SerializeField] int indexProjectile; //index of the prefab to pass on when picked up and
                                          //enable that projectile 

    
    [SerializeField] AudioSource getPickupSound;

    private void Start()
    {
        getPickupSound = GameObject.FindGameObjectWithTag("getPickupSound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.gameObject.tag == "Player")
        {            
            other.GetComponent<ShootingPlayer>().EnableProjectileButton(indexProjectile);
            CommonElements();
        } else if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<ShootingEnemy>().AddProjectileToList(indexProjectile);
            CommonElements();
        }
    }

    private void CommonElements()
    {
        getPickupSound.Play();
        Destroy(gameObject);
    }
}
