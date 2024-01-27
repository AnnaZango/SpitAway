using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAiming : MonoBehaviour
{
    // This script controls the player aiming, shooting and if it has the turn or not

    [SerializeField] float currentPower;
    [SerializeField] float currentAngle;
    [SerializeField] GameObject aimSpriteGameObject;
    [SerializeField] GameObject strengthShotSprite;

    [SerializeField] ShootingPlayer shootingPlayer;

    [SerializeField] bool isPlayerTurn = true;
    [SerializeField] bool canAct = false;
    [SerializeField] Timer timer;

    [SerializeField] GameObject head;
    [SerializeField] Slider sliderAngle;
    [SerializeField] float maximumRotationZ = 45f;

    [SerializeField] Slider sliderPower;

    [SerializeField] Animator spritesAlpacaAnimator;

    private void OnEnable()
    {
        Timer.OnTimerUp += TimeTurnIsUp;
    }
    private void OnDisable()
    {
        Timer.OnTimerUp -= TimeTurnIsUp;
    }

    private void TimeTurnIsUp()
    {
        //if time is up and player did not shoot, sliders disabled and angle reset and animation
        //set to idle.

        if (!canAct) { return; }
        canAct = false;

        sliderAngle.enabled = false;
        sliderPower.enabled = false;

        Invoke(nameof(ResetAngleAndSetIdle), 1f);
    }

    
    void Start()
    {
        GameManager.SetGameFinished(false);
        timer = FindObjectOfType<Timer>();

        //assign respective methods to sliders
        sliderAngle.onValueChanged.AddListener(delegate
        {
            Rotate();
        });
        sliderPower.onValueChanged.AddListener(delegate
        {
            ChangePower();
        });

        canAct = false;
        Invoke(nameof(SetCanAct), 1f); //slight delay to enable player control when everything is set
    }

    

    public void Rotate()
    {
        //we rotate the alpaca's head (slider goes from -1 to 1 and it multiplies by max rotation
        float angle = sliderAngle.value * maximumRotationZ;
        head.transform.localEulerAngles = new Vector3(0, 0, angle);
        currentAngle = angle;        
    }

    public void ChangePower()
    {
        currentPower = sliderPower.value;
    }

    public void StartShooting() //assigned to shoot button in scene, starts shooting process, 
                                // finishes turn and disables actions/buttons
    {
        if (GameManager.GetIfGameFinished()) { return; }

        if (!isPlayerTurn || !canAct) { return; }

        timer.StopTimer();
        canAct = false;//to avoid shooting twice
        isPlayerTurn = false;

        sliderAngle.enabled = false; //sliders only available if player turn
        sliderPower.enabled = false;

        spritesAlpacaAnimator.SetTrigger("shoot");
    }


    //Called by ShootEvent at appropriate time
    public void ShootProjectile()
    {
        shootingPlayer.FireProjectile(currentPower); //projectile shot from current angle
                                                     //at current power              

        Invoke(nameof(ResetAngleAndSetIdle), 1f);
    }

    

    private void ResetAngleAndSetIdle()
    {
        currentAngle = 0;
        sliderAngle.value = currentAngle;
        head.transform.localEulerAngles = new Vector3(0, 0, currentAngle);

        spritesAlpacaAnimator.SetBool("move", false);
        spritesAlpacaAnimator.SetBool("jump", false);     
    }

    public bool GetIfCanAct()
    {
        return canAct;
    }

    private void SetCanAct()
    {        
        canAct = true;
    }

    public void SetTurn(bool isTurn)
    {
        isPlayerTurn = isTurn;
        if (isTurn)
        {
            canAct = true;
            timer.StartTimer();
            sliderAngle.enabled = true;
            sliderPower.enabled = true;
        }
    }

    public bool GetIfTurnActive()
    {
        return isPlayerTurn;
    }
}
