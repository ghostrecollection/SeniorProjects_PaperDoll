using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CleaningBrushInteraction : MonoBehaviour, IInteractable
{
    // SOME TEMP HARDCODED ELEMENTS TO FIX LATER.
    // -------------------------------------
    [Header("Input:")]
    [Space(10)]
    // INPUT
    [SerializeField] PlayerInput playerInput;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject playerReference;

    [Header("Canvas Components:")]
    [Space(10)]
    // INPUT VISUAL PROMPT
    [SerializeField] GameObject interactPrompt;
    // SLIDER
    [SerializeField] GameObject sliderScreen;
    [SerializeField] Slider slider;

    [Header("Ink Components:")]
    [Space(10)]
    [SerializeField] Material inkMat;
    [SerializeField] bool startFade = false;

    [Header("Time:")]
    [Space(10)]
    private float elapsedTime = 0f;
    [SerializeField] float targetDissolveTime = 5f;
    [SerializeField] float currentValue = 0f;
    [SerializeField] float targetValue = 1f;

    [Header("Cameras:")]
    [Space(10)]
    // INTERACTION CAM AND PRIORITY
    [SerializeField] public CinemachineCamera interactCam; // Camera to change to upon interacting.
    [SerializeField] int interactPriorityNum; // Priority Number that is HIGHER THAN the standard CameraChangesManager priority number ->
    // <- This way the interact can switch to the new camera while preserving the camera switches from zone colliders.

    [Space(10)]

    [Header("Interaction Parameters:")]
    [Space(10)]
    // BOOLS
    [HideInInspector] bool interactDone = false;
    [HideInInspector] public bool withinInteract = false;
    // PLAYER TARGET POSITION
    [SerializeField] GameObject targetOrientation;
    private Vector3 interactPosition;
    private Vector3 interactRotation;
    // -------------------------------------

    // Specific INK interaction outcomes using my interaction system as structure:

    // --- START ---
    public void Start()
    {
        // Finds the player game object.
        playerReference = GameObject.FindWithTag("Player");
        // Renderer.
        Renderer renderer = GetComponent<Renderer>();
        inkMat = renderer.material;
    }


    // --- INTERACT ---
    public void Interact()
    {
        // Ensures the input visual prompt is turned off upon interacting.
        interactPrompt.SetActive(false);

        // Changes the game state.
        GameStateController.instance.ChangeState("MOUSE");
        // DEBUG --- Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");

        if (!interactDone)
        {
            withinInteract = true;

            // Switches to new camera.
            interactCam.Priority = interactPriorityNum;

            //MovePlayerPosition();

            sliderScreen.SetActive(true);

            if(slider.value > 0)
            {
                elapsedTime = 0;
                startFade = true;
                StartCoroutine(Wait());
            }
        }
    }


    // --- ON HOVER ---
    public void OnHover()
    {
        if (!withinInteract)
        {
            // Turns on the canvas component when the mouse hovers.
            interactPrompt.SetActive(true);
        }
    }


    // --- ON HOVER EXIT ---
    public void OnHoverExit()
    {
        // Turns off canvas component when the mouse leaves.
        interactPrompt.SetActive(false);
    }


    // --- UPDATE ---
    public void Update()
    {
        if(startFade) InkFade();

        // Sets the game state qualities.
        // I will likely move this in the future.
        if (GameStateController.instance.gstate.state == GameState.gameState.MOUSE)
        {
            playerInput.actions.FindAction("Move").Disable();
            playerInput.actions.FindAction("Run").Disable();
            playerInput.actions.FindAction("Jump").Disable();
        }

        if(GameStateController.instance.gstate.state == GameState.gameState.NORMAL)
        {
            playerInput.actions.FindAction("Move").Enable();
            playerInput.actions.FindAction("Run").Enable();
            playerInput.actions.FindAction("Jump").Enable();
        }
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        interactDone = true;

        if (interactDone)
        {
            interactCam.Priority = 0;

            withinInteract = false;
            interactDone = false;
            startFade = false;

            // Changes game state back to normal.
            GameStateController.instance.ChangeState("NORMAL");
            // DEBUG --- Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");
        }
    }

    void InkFade()
    {
        if (elapsedTime < targetDissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / targetDissolveTime;
            float value = Mathf.Lerp(currentValue, targetValue, t);
            inkMat.SetFloat("_AlphaClipThreshold", value);
        }
    }


    /*void MovePlayerPosition()
    {
        interactPosition = targetOrientation.transform.position;
        interactRotation = targetOrientation.transform.eulerAngles;

        // Add some sort of player target position and rotation for them to move to.
        // Will polish in another pass -- add lerping etc.
        playerReference.transform.position = interactPosition;
        playerReference.transform.eulerAngles = interactRotation;
    }*/
}
