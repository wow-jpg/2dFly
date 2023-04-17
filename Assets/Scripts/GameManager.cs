using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSigleton<GameManager>
{
    public static Action onGameOver;


    public static GameState GameState
    {
        get
        {
            return Instance.gameState;

        }


        set
        {
            Instance.gameState = value;
        }

    }
    [SerializeField] GameState gameState = GameState.Playing;


}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}
