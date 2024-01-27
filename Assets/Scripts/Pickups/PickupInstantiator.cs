using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInstantiator : MonoBehaviour
{
    //This script is used to instantiate projectiles and hearts at the beginnint of player/enemy turn.
    //There are two instantiators, one for the player and one for the enemy. Each contains a list of
    //potential projectiles and hearts (currently one type, there could be more) which will be 
    //instantiated within a given range (set by limitLeft and limitRight) at runtime.

    [SerializeField] Transform limitLeft;
    [SerializeField] Transform limitRight;

    [SerializeField] PlayerStats player;
    [SerializeField] EnemyAI enemy;
    [SerializeField] bool playerZone;

    [SerializeField] List<GameObject> pickupsHearts = new List<GameObject>();
    [SerializeField] List<GameObject> pickupsProjectiles = new List<GameObject>();

    [SerializeField] AudioSource soundPickupAppear;


    private Vector3 SetRandomLocation() //pickup istantiated at a random position within range
    {
        var position = new Vector3(Random.Range(limitLeft.transform.position.x,
            limitRight.transform.position.x), transform.position.y, 0);
        return position;
    }

    public void InstantiateProjectilePickup()
    {
        if (pickupsProjectiles.Count == 0) { return; }

        //1/3 chance to instantiate:
        int randomProbability = Random.Range(0, 3);   
        
        if(randomProbability == 0)
        {
            int indexRandom = Random.Range(0, pickupsProjectiles.Count);

            GameObject instancePrefab = Instantiate(pickupsProjectiles[indexRandom], SetRandomLocation(),
                Quaternion.identity);

            instancePrefab.transform.parent = gameObject.transform;
            soundPickupAppear.Play();
            //remove projectile already instantiated from list, because projectile available from now on.
            pickupsProjectiles.RemoveAt(indexRandom); 
        }                      
    }

    public void InstantiateHeartPickup() //chance of instantiating depends on current health
    {
        int need = GetHeartNeed();

        int randomProbability;
        
        switch (need)
        {
            case (1):
                randomProbability = Random.Range(0, 50); //2% chance
                break;
            case (2):
                randomProbability = Random.Range(0, 20); //5% chance
                break;
            case (3):
                randomProbability = Random.Range(0, 5); //20% chance
                break;
            default:
                randomProbability = Random.Range(0, 10); //10% chance default
                break;
        }

        
        if (randomProbability == 0) //only instantiate if randomProbability = 0
        {
            int indexRandom = Random.Range(0, pickupsHearts.Count);

            GameObject instancePrefab = Instantiate(pickupsHearts[indexRandom], SetRandomLocation(),
                Quaternion.identity);
            instancePrefab.transform.parent = gameObject.transform;
            soundPickupAppear.Play();
        }
    }
    
    private int GetHeartNeed()
    {
        int need = 0; //the lower the health, the higher the need
        int currentHealth;
        if (playerZone)
        {
            currentHealth = player.GetCurrentHealth();
        }
        else
        {
            currentHealth = enemy.GetCurrentHealth();
        }

        if (currentHealth == 1)
        {
            need = 3;
        }
        else if (currentHealth >= 4)
        {
            need = 1;
        }
        else
        {
            need = 2;
        }

        return need;
    }
}
