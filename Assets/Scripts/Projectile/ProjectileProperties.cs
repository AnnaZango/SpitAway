using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    // This script is attached to the projectile prefab and determines its properties

    [SerializeField] int damagePoints = 1;
    [SerializeField] float timeAlive = 10;
    [SerializeField] float damageRadius = 1;
    [SerializeField] float timeParticles = 0.5f;
    [SerializeField] GameObject prefabVFX;

   

    public int GetDamagePoints()
    {
        return damagePoints;
    }
    public float GetTimeAlive()
    {
        return timeAlive;
    }
    public float GetDamageRadius()
    {
        return damageRadius;
    }
    public GameObject GetVFX()
    {
        return prefabVFX;
    }
    public float GetTimeParticles()
    {
        return timeParticles;
    }

}
