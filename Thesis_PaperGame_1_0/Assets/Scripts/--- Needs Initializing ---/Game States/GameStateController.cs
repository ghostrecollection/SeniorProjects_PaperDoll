using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController instance {  get; private set; }
    public GameState gstate;
    public List<IGameStateManager> stateObjs;


    // --- AWAKE ---
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is already instance of game state controller in this scene! Removing previous instance.");
        }

        instance = this;
        gstate = new GameState();
    }


    // --- START ---
    void Start()
    {
        stateObjs = GetAllGameStateObjs();
    }


    // --- CHANGE STATE ---
    public void ChangeState(string state)
    {
        switch (state)
        {
            case "NORMAL":
                {
                    gstate.state = GameState.gameState.NORMAL;
                    break;
                }
            case "DIALOGUE":
                {
                    gstate.state = GameState.gameState.DIALOGUE;
                    break;
                }
            case "MENU":
                {
                    gstate.state = GameState.gameState.MENU;
                    break;
                }

        }

        foreach (IGameStateManager stateObj in stateObjs)
        {
            stateObj.GetState(gstate);
        }
    }


    // --- LIST ---
    public List<IGameStateManager> GetAllGameStateObjs()
    {
        IEnumerable<IGameStateManager> stateManagers = FindObjectsByType<MonoBehaviour>().OfType<IGameStateManager>();
        return new List<IGameStateManager>(stateManagers);
    }

}
