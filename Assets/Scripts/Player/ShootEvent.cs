using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEvent : MonoBehaviour
{
    [SerializeField] PlayerAiming playerAiming;

    void Start()
    {
        playerAiming = FindObjectOfType<PlayerAiming>();
    }


    //Called by animation event in shoot animation, when alpaca is ready to shoot
    public void Shoot()
    {
        playerAiming.ShootProjectile();
    }
}
