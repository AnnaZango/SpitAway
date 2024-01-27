using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnController : MonoBehaviour
{
    //This script contains the methods to assign the turn

    [SerializeField] PlayerAiming player1;
    [SerializeField] PlayerAiming player2;
    [SerializeField] EnemyAI player2AI;
    [SerializeField] bool isPlayerTurn;
    CameraMovement cameraMovement;

    [SerializeField] PickupInstantiator playerPickups;
    [SerializeField] PickupInstantiator enemyPickups;

    void Start()
    {
        isPlayerTurn = true;
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

   
    public bool GetIfPlayerTurn()
    {
        return isPlayerTurn;
    }    

    public void ChangeTurn(bool toPlayer)
    {
        if (toPlayer)
        {
            player1.gameObject.GetComponent<PlayerAiming>().SetTurn(true);
            player2AI.gameObject.GetComponent<EnemyAI>().SetTurn(false);
            playerPickups.InstantiateHeartPickup();
            playerPickups.InstantiateProjectilePickup();
            isPlayerTurn = true;
        }
        else
        {
            player1.gameObject.GetComponent<PlayerAiming>().SetTurn(false);
            player2AI.gameObject.GetComponent<EnemyAI>().SetTurn(true);
            enemyPickups.InstantiateProjectilePickup();
            enemyPickups.InstantiateHeartPickup();
            isPlayerTurn = false;
        }
    }

}
