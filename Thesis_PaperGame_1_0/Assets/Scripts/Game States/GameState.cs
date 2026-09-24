using UnityEngine;

public class GameState 
{
    public enum gameState
    {
        NORMAL,
        DIALOGUE,
        MENU
    }

    public gameState state;

    public GameState()
    {
        state = gameState.NORMAL;
    }
}
