using UnityEngine;

public class ChildTriggerRelay : MonoBehaviour
{
    // -------------------------------------
    [Header("Parent References:")]
    [Space(10)]
    // SCRIPTS
    [SerializeField] private PlayerCharacterController parentCharacterController;
    // PLAYER REFERENCE
    [SerializeField] GameObject playerReference;
    // -------------------------------------


    // --- START ---
    void Start()
    {
        // Finds the player game object.
        playerReference = GameObject.FindWithTag("Player");
        // Get PlayerCharacterController Script from Player Reference.
        parentCharacterController = playerReference.GetComponent<PlayerCharacterController>();
    }


    // --- ON TRIGGER ENTER ---
    void OnTriggerEnter(Collider other)
    {
        parentCharacterController.OnChildTriggerEnter(other, gameObject);
    }


    // --- ON TRIGGER EXIT --- 
    void OnTriggerExit(Collider other)
    {
        parentCharacterController.OnChildTriggerExit(other, gameObject);
    }
}
