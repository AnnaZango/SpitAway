using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLocation : MonoBehaviour
{
    //This script is used to set a random location for the enemy to shoot from.

    [SerializeField] GameObject locationLeft; //left limit of enemy area
    [SerializeField] GameObject locationRight; //right limit of enemy area

    public void SetALocation()
    {
        //Random position between the limit locations of enemy's area
        var position = new Vector3(Random.Range(locationLeft.transform.position.x, 
            locationRight.transform.position.x), 0, 0);
        gameObject.transform.position = position;       
    }
}
