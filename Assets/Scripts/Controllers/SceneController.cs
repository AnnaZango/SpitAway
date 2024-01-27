using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //This script is used to control going from one scene to the other and quit the game

    public void LoadSceneGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit() //works on standalone and android
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
