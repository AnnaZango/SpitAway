using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    //This script controls the panel to show at the end of the game

    [SerializeField] GameObject panelWin;
    [SerializeField] GameObject panelLose;
    [SerializeField] AudioSource winSound;
    [SerializeField] AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        panelWin.SetActive(false);
        panelLose.SetActive(false);
    }

    public void ShowPanelEnd()
    {
        StartCoroutine(ShowPanelDelay());
    }

    IEnumerator ShowPanelDelay()
    {
        //wait a few seconds to show dead animation and set panel accordin to win or lose condition
        yield return new WaitForSeconds(3);

        if (GameManager.GetIfPlayerWins())
        {
            panelWin.SetActive(true);
            winSound.Play();
        }
        else
        {
            panelLose.SetActive(true);
            loseSound.Play();
        }
    }
}
