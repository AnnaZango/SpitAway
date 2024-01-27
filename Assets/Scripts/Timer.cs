using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    //This script controls the timer for the player

    [SerializeField] float timeTurn = 20;
    [SerializeField] float currentTime = 20;
    [SerializeField] TextMeshProUGUI textTimer;

    public static Action OnTimerUp;

    CameraMovement cameraMovement;

    private void Awake()
    {
        cameraMovement = FindObjectOfType<CameraMovement>();
    }


    IEnumerator DecreaseTimer() //less consuming than doing this on Update
    {
        if (GameManager.GetIfGameFinished()) { StopCoroutine(nameof(DecreaseTimer)); }
        currentTime--;
        textTimer.text = currentTime.ToString();

        if (currentTime <= 0)
        {
            TimeIsOver();
        }
        else
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(nameof(DecreaseTimer));
        }
    }

    private void TimeIsOver()
    {
        OnTimerUp?.Invoke();
        SetCameraToEnemy(); //if time is up and player did not shoot, camera goes to enemy
        StopTimer();
    }

    public void StopTimer()
    {
        currentTime = 0;
        textTimer.text = "";
        StopCoroutine(nameof(DecreaseTimer));
    }

    public void SetCameraToEnemy()
    {
        cameraMovement.SetCharacterTargetNull(); //first set no character target so it does not go to player
        Invoke(nameof(ChangeCameraToEnemy), 1f);
    }

    private void ChangeCameraToEnemy()
    {
        cameraMovement.SetEnemyTarget();
    }

    public void StartTimer()
    {
        currentTime = timeTurn;
        StartCoroutine(nameof(DecreaseTimer));
    }
   

}
