using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
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
    [SerializeField] InteractionSystem interactionSystem;

    [Header("Canvas Components:")]
    [Space(10)]
    // INPUT VISUAL PROMPT
    [SerializeField] GameObject interactPrompt;
    [SerializeField] GameObject cleaningScreen;

    [Header("Ink Components:")]
    [Space(10)]
    [SerializeField] Material inkMat;

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
    // MOUSE
    [HideInInspector] bool holdingMouse;
    [HideInInspector] private float targetDistance = 0f;
    [HideInInspector] private float virtualDistance = 0f;
    [HideInInspector] private float maxDistance;
    [SerializeField] float sensitivity = 0.5f;


    // -------------------------------------

    // Specific INK interaction outcomes using my interaction system as structure:

    // --- START ---
    public void Start()
    {
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
        Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");

        if (!interactDone)
        {
            withinInteract = true;

            // Switches to new camera.
            interactCam.Priority = interactPriorityNum;

            cleaningScreen.SetActive(true);

            //MovePlayerPosition();
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

        // Counting Logic:
        if (withinInteract)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                holdingMouse = true;
                Debug.Log("Holding Mouse");
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                holdingMouse = false;
                Debug.Log("Released Mouse");
            }

            if (holdingMouse && Mouse.current.leftButton.isPressed)
            {
                // NOTE: Sensitivity currently not really working.. will work on later;
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();

                virtualDistance += mouseDelta.magnitude * sensitivity;

                virtualDistance = Mathf.Max(0f, virtualDistance);

                maxDistance = targetDistance > 0f ? targetDistance : Screen.width/3;

                float alphaClipValue = Mathf.InverseLerp(0f,maxDistance, virtualDistance);

                inkMat.SetFloat("_AlphaClipThreshold", alphaClipValue);
                // Debug.Log($"Distance: {distance}");

                if(alphaClipValue > 0.7f) interactDone = true;
            }

            if (interactDone)
            {
                StartCoroutine(Wait());
            }
        }
    }


    // --- WAIT --- 
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        interactDone = true;

        if (interactDone)
        {
            cleaningScreen.SetActive(false);

            // Changes game state back to normal.
            GameStateController.instance.ChangeState("NORMAL");
            Debug.Log($"State: {GameStateController.instance.gstate.state.ToString()}");
            playerInput.actions.FindAction("Move").Enable();
            playerInput.actions.FindAction("Run").Enable();
            playerInput.actions.FindAction("Jump").Enable();

            interactCam.Priority = 0;

            withinInteract = false;
            interactDone = false;
        }

        // Ink interaction is complete an turns off the object!
        if(!interactDone && GameStateController.instance.gstate.state == GameState.gameState.NORMAL)
        {
            gameObject.SetActive(false);
        }
    }
}
