using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
    void OnHover();
    void OnHoverExit();
}
public class InteractionSystem : MonoBehaviour
{
    // -------------------------------------
    // INPUT REFERENCES
    // Script reference for Player Inputs
    [Header("Player Input References:")]
    [Space(10)]
    // SCRIPTS
    [SerializeField] private PlayerCharacterController playerCharacterController;
    [SerializeField] private CleaningBrushInteraction interactionTypeOne;
    // PLAYER REFERENCE
    [SerializeField] GameObject playerReference;
    [SerializeField] bool hasInteracted = false;

    [Space(10)]
    // MOUSE INPUT
    [Header("Mouse Position Input Setup:")]
    [Space(10)]
    [SerializeField] private Camera cameraRoot;
    [SerializeField] private LayerMask layerMask;

    [Space(10)]
    // VISUAL FEEDBACK
    [Header("Visual Feedback:")]
    [Space(10)]
    // Temporary Visual Feedback for Interaction.
    public Color hoverColor = Color.lightCoral;
    private Color originalColor;
    private Renderer renderer;
    
    [Space(10)]
    // INTERFACE
    private IInteractable interactCurrent;
    // -------------------------------------


    // --- START ---
    private void Start()
    {
        // Finds the player game object.
        playerReference = GameObject.FindWithTag("Player");
        // Get PlayerCharacterController Script from Player Reference.
        playerCharacterController = playerReference.GetComponent<PlayerCharacterController>();
    }


    // --- UPDATE ---
    void Update()
    {
        // Calls Mouse Input interaction.
        CastInteraction();
    }


    // --- INTERACT ---
    void CastInteraction()
    {
        // Reads the mouses position as a Vector3.
        Vector3 mousePos = Mouse.current.position.ReadValue();
        // Creates a ray from the mouse position to the camera.
        Ray ray = cameraRoot.ScreenPointToRay(mousePos);

        // Ray according to where it hits the included Layer Masks.
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            // Moves to the positon from the hit location.
            transform.position = hit.point;
            // DEBUG --- Debug.Log(mousePos);

            // If the ray hits...
            if (hit.transform.gameObject.TryGetComponent(out IInteractable i) && playerCharacterController.withinInteractDist == true)
            {
                if (i != interactCurrent)
                {
                    Clear();
                    interactCurrent = i;
                    // Currently hovering over an interactable object.
                    interactCurrent.OnHover();

                    // Temporary visual feedback to indicate that the mouse is hovering.
                    // This visual feedback will be changed in a future iteration to suit the art direction.
                    renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer && !interactionTypeOne.withinInteract)
                    {
                        originalColor = renderer.material.color;
                        renderer.material.color = hoverColor;
                    }
                }
                
                // While hovering, if the player inputs interaction (LeftClick or "E" key)->
                if (playerCharacterController.attemptInteraction && !hasInteracted)
                {
                    // Removes the hover color.
                    if (renderer) renderer.material.color = originalColor;

                    // <- Calls interact
                    interactCurrent.Interact();
                    hasInteracted = true;
                    // Resetting the bool quicker here.
                    playerCharacterController.attemptInteraction = false;
                }
                return;
            }
            else
            {
                // Calls the Clear function.
                Clear();
            }
        }
    }


    // --- CLEAR ---
    void Clear()
    {
        if (interactCurrent != null)
        {
            // If the mouse stops hovering over an interactable->
            interactCurrent?.OnHoverExit();
            // <-removes the visual feedback that the mouse is over an interactable object.
            if (renderer) renderer.material.color = originalColor;
            interactCurrent = null;
            renderer = null;

            hasInteracted = false;
        }
        
    }

}
