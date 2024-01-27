using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //This script is assigned to the camera and it is responsible for moving it following either 
    //the player/enemy (if it is their turn), or the projectile shot

    [SerializeField] Transform projectileTarget;
    [SerializeField] Transform characterTarget;
    [SerializeField] Transform enemy;
    [SerializeField] Transform player;
    [SerializeField] float limitleft = 110;
    [SerializeField] float limitRight = -2;

    bool reachedPlayer = false;

    TurnController turnController;
    
    void Start()
    {
        turnController = FindObjectOfType<TurnController>();

        //follow player at start
        transform.position = new Vector3(characterTarget.position.x,
                    characterTarget.position.y + 1, transform.position.z);
    }


    void Update()
    {
        if (GameManager.GetIfGameFinished()) { return; }
        if(projectileTarget == null) //if there is no projectile, it must go towards player/enemy
        {
            if (characterTarget == null) { return; }

            //character target, slightly above character on y and maintaining camera's z position
            Vector3 goalCharacterTarget = new Vector3(
            characterTarget.position.x, characterTarget.position.y + 1, transform.position.z);

            float distanceToTarget = Vector3.Distance(transform.position, goalCharacterTarget);

            if (distanceToTarget < 0.1f) // if it is close to player, change turn if needed. This 
                //happens after the camera follows a projectile, hits something, and must then go
                //to the caracter who has the next turn
            {
                if (reachedPlayer) { return; }

                reachedPlayer = true; 

                //set turn of player or enemy
                if (characterTarget.gameObject.GetComponent<PlayerAiming>())
                {
                    turnController.ChangeTurn(true);
                }
                else 
                {
                    turnController.ChangeTurn(false);
                }
                
            }
            else //if it is far, camera goes smoothly towards the player
            {
                transform.position = Vector3.Lerp(transform.position, goalCharacterTarget, Time.deltaTime * 2);                
            }

        }
        else
        {
            //if there is a projectile, camera must always follow it to see where it hits
            CameraFollowProjectile();
            reachedPlayer = false;
        }
    }

    public void SetNewProjetileTarget(Transform newTarget)
    {
        projectileTarget = newTarget;
    }
        
    public void SetCharacterTargetNull()
    {
        characterTarget = null;
        reachedPlayer = false;
    }

    public void SetEnemyTarget()
    {
        projectileTarget = null;
        characterTarget = enemy;
    }
    public void SetPlayerTarget()
    {
        projectileTarget = null;
        characterTarget = player;
    }

    private void CameraFollowProjectile()
    {
        characterTarget = null;
        Vector3 goalTarget = new Vector3(
            Mathf.Clamp(projectileTarget.position.x, limitleft, limitRight),             
            projectileTarget.position.y,
            transform.position.z);
        Vector3 positionTarget = new Vector3(projectileTarget.position.x, projectileTarget.position.y,
            transform.position.z);

        transform.position = positionTarget; //camera follows projectile's exact position  (except z)     
    }   

}
