using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState gameState = GameState.Playing;
    public GameState GameState => gameState;
    
    public void ChangeGameState(GameState state)
    {
        gameState = state;
    }
}

public enum GameState
{
    Playing,
    Paused,
}
