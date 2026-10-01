using UnityEngine;

public class GameState 
{
    public enum gameState
    {
        NORMAL,
        MOUSE,
        DIALOGUE,
        MENU
    }

    public gameState state;

    public GameState()
    {
        state = gameState.NORMAL;
    }
}
