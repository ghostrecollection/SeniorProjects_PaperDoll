using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public UnityEvent trigger;
    // Script reference for Player Inputs
    [SerializeField] private PlayerInputManager pInput;
    [SerializeField] private PlayerCharacterController playerCharacterController;
    [SerializeField] GameObject playerReference;

    [Header("Interaction References")]
    [Space(10)]
    [SerializeField] GameObject interactionScreen;
    [SerializeField] GameObject sliderScreen;
    [SerializeField] GameObject inkPileInstance;

    public bool canInteract;


    // --- START ---
    public void Start()
    {
        if(playerReference != null)
        {
            // Get PlayerInputManager Script from GameObject.
            pInput = playerReference.GetComponent<PlayerInputManager>();
            // Get PlayerCharacterController Script from GameObject.
            playerCharacterController = playerReference.GetComponent<PlayerCharacterController>();
        }
        else
        {
            Debug.LogError("Player Reference is missing in the Inspector!", this);
        }
        
    }


    // --- UPDATE --- 
    public void Update()
    {
        TurnOnCanvas();
    }


    // --- ON TRIGGER ENTER ---
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ink"))
        {
            canInteract = false;
        }

        if (other.CompareTag("Player"))
        {
            if (inkPileInstance == true)
            {
                interactionScreen.SetActive(true);
                canInteract = true;
            }
            else
            {
                interactionScreen.SetActive(false);
            }
        }
    }


    // --- ON TRIGGER EXIT ---
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionScreen.SetActive(false);
            canInteract = false;
        }
    }


    // --- TURN ON CANVAS ---
    public void TurnOnCanvas()
    {
        if (playerCharacterController.attemptInteraction && canInteract == true)
        {
            Debug.Log("Pressed E");
            sliderScreen.SetActive(true);
        }
        
    }


    // --- REMOVE INK ---
    public void RemoveInk()
    {
        inkPileInstance.SetActive(false);
        
    }
}
