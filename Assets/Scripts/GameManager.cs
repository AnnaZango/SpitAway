using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager 
{
    //General script to control global variables which must be accessed from many scripts

    public static bool hasPlayerWon = false;
    public static bool isGameFinished = false;

    public static bool GetIfPlayerWins()
    {
        return hasPlayerWon;
    }
    public static void SetPlayerWins(bool alive)
    {
        hasPlayerWon = alive;
    }

    public static bool GetIfGameFinished()
    {
        return isGameFinished;
    }
    public static void SetGameFinished(bool gameFinished)
    {
        isGameFinished = gameFinished;
    }

}
