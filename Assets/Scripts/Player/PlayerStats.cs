using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // This script controls player's health

    [SerializeField] int maxHhealthPoints = 3;
    [SerializeField] int currentHealth = 3;
    [SerializeField] GameObject[] hearts;

    [SerializeField] AudioSource hurtSound;
    SpriteRenderer[] sprites;

    [SerializeField] Animator spritesAlpacaAnimator;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        currentHealth = maxHhealthPoints;
        UpdateHearts();
    }
    

    public void ChangeHealthPoints(int pointsToAdd) 
    {
        //Method to increase or decrease health

        currentHealth += pointsToAdd;
        if (pointsToAdd < 0)
        {
            hurtSound.Play();
            if (currentHealth <= 0)
            {
                //die
                spritesAlpacaAnimator.SetTrigger("die");
                GameManager.SetGameFinished(true);
                GameManager.SetPlayerWins(false);
                FindObjectOfType<EndGameController>().ShowPanelEnd();
            }
            else
            {
                Hurt();
            }            
        }
        SetHealthWithinRange();
        UpdateHearts();
    }

    private void Hurt()
    {
        //all sprites turn red
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.color = new Color32(255, 130, 130, 255);
        }
        Invoke(nameof(ResetSpritesColor), 1f);
    }

    private void ResetSpritesColor()
    {
        //all sprites back to white
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
    }


    private void SetHealthWithinRange()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;            
        }
        else if (currentHealth > maxHhealthPoints)
        {
            currentHealth = maxHhealthPoints;
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i > currentHealth-1)
            {
                hearts[i].SetActive(false);
            }
            else
            {
                hearts[i].SetActive(true);
            }
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
