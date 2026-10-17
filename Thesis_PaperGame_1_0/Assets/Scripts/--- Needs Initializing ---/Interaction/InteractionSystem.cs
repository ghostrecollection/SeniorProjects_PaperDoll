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
    [SerializeField] private PlayerInputManager pInput;
    [SerializeField] GameObject playerReference;


    public Camera cameraRoot;
    public float distance = 3.0f;
    public Color hoverColor = Color.lightCoral;

    private IInteractable current;
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
        RaycastHit hit;
        if(Physics.Raycast(cameraRoot.transform.position, cameraRoot.transform.forward, out hit) && hit.collider.TryGetComponent(out IInteractable i))
        {
            if(current != i)
            {
                Clear();
                current = i;
                current.OnHover();

                renderer = hit.collider.GetComponent<Renderer>();
                if (renderer)
                {
                    originalColor = renderer.material.color;
                    renderer.material.color = hoverColor;
                }

                if (pInput.interact)
                {
                    current.Interact();
                }
                // if (Keyboard.current[Key.E].wasPressedThisFrame) current.Interact();
            }
            else
            {
                Clear();
            }

            void Clear()
            {
                if (current != null) current.OnHoverExit();
                if(renderer) renderer.material.color = originalColor;
                current = null;
                renderer = null;
            }

        }
    }
}
