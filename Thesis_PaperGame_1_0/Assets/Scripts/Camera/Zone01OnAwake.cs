using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Zone01OnAwake : MonoBehaviour
{
    [Header("Input:")]
    [Space(10)]
    // INPUT
    [SerializeField] PlayerInput playerInput;
    public PlayableDirector director;

    [Header("Cameras:")]
    [Space(10)]
    // INTERACTION CAM AND PRIORITY
    [SerializeField] public CinemachineCamera cutsceneCam; // Camera to change to upon interacting.
    [SerializeField] int interactPriorityNum; // Priority Number that is HIGHER THAN the standard CameraChangesManager priority number ->
    // <- This way the interact can switch to the new camera while preserving the camera switches from zone colliders.

    private void Start()
    {
        cutsceneCam.Priority = interactPriorityNum;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Changes the game state.
        GameStateController.instance.ChangeState("MOUSE");
        //GameStateController.instance.ChangeState("MOUSE");
        Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");
    }

    private void OnEnable()
    {
        if (director != null)
        {
            director.stopped += OnCutsceneFinished;
        }
    }

    private void OnDisable()
    {
        if(director != null)
        {
            director.stopped -= OnCutsceneFinished;
        }
    }

    public void StartCutscene()
    {
        director.Play();
    }

    private void Update()
    {
        // Sets the game state qualities.
        // I will likely move this in the future.
        if (GameStateController.instance.gstate.state == GameState.gameState.MOUSE)
        {
            playerInput.actions.FindAction("Move").Disable();
            playerInput.actions.FindAction("Run").Disable();
            playerInput.actions.FindAction("Jump").Disable();
        }

        if (GameStateController.instance.gstate.state == GameState.gameState.NORMAL)
        {
            playerInput.actions.FindAction("Move").Enable();
            playerInput.actions.FindAction("Run").Enable();
            playerInput.actions.FindAction("Jump").Enable();
        }

        
    }

    private void OnCutsceneFinished(PlayableDirector aDirector)
    {
        if (aDirector != director) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Changes game state back to normal.
        GameStateController.instance.ChangeState("NORMAL");
        Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");
        playerInput.actions.FindAction("Move").Enable();
        playerInput.actions.FindAction("Run").Enable();
        playerInput.actions.FindAction("Jump").Enable();

        cutsceneCam.Priority = 0;
    }

    

}
