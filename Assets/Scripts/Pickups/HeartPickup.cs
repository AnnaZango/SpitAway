using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    // Script to give live to player/enemy on pickup 

    [SerializeField] int healthToAdd = 1;

    [SerializeField] AudioSource getPickupSound;

    void Start()
    {
        getPickupSound = GameObject.FindGameObjectWithTag("getPickupSound").GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.tag == "Player") { FindObjectOfType<PlayerStats>().ChangeHealthPoints(healthToAdd); }
            if(other.gameObject.tag == "Enemy") { FindObjectOfType<EnemyAI>().ChangeHealthPoints(healthToAdd); }
            
            getPickupSound.Play();
            Destroy(transform.GetChild(0).gameObject);            
            Destroy(gameObject);
        } 
    }
}
