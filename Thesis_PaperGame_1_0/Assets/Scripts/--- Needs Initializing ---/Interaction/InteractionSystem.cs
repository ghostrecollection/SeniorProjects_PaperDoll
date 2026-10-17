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
    [SerializeField] private PlayerInputManager pInput;
    //[SerializeField] PlayerInput pInputReader;
    [SerializeField] GameObject playerReference;

    [Space(10)]

    [Header("Mouse Position Input Setup:")]
    [Space(10)]
    [SerializeField] private Camera cameraRoot;
    [SerializeField] private LayerMask layerMask;

   
    //public float distance = 3.0f;
    public Color hoverColor = Color.lightCoral;

    private IInteractable interactCurrent;
    private Renderer renderer;
    private Color originalColor;
    // -------------------------------------


    // --- START ---
    private void Start()
    {
        // Finds the player game object.
        playerReference = GameObject.FindWithTag("Player");
        // Get PlayerInputManager Script from PlayerReference.
        PlayerInputManager pInput = playerReference.GetComponent<PlayerInputManager>();
    }


    // --- UPDATE ---
    void Update()
    {
        // Reads the mouses position as a Vector3.
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cameraRoot.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            transform.position = hit.point;
            // DEBUG --- Debug.Log(mousePos);

            if(hit.collider.TryGetComponent(out IInteractable i))
            {
                if (interactCurrent != i)
                {
                    Clear();
                    interactCurrent = i;
                    interactCurrent.OnHover();

                    renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer)
                    {
                        originalColor = renderer.material.color;
                        renderer.material.color = hoverColor;
                    }


                    // CURRENTLY NOT WORKING
                    // I think its setting it to true too quickly and too quickly back to false.
                    if (pInput.interact)
                    {
                        Debug.Log("Interacted With Object!");
                        interactCurrent.Interact();
                        pInput.interact = false;
                    }
                }
            }
            else
            {
                Clear();
            }
        }
    }

    void Clear()
    {
        interactCurrent?.OnHoverExit();
        if (renderer) renderer.material.color = originalColor;
        interactCurrent = null;
        renderer = null;
    }
}
