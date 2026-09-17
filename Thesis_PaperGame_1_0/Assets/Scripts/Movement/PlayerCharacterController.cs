using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    // CHARACTER CONTROLLER
    private CharacterController controller;

    // MOVEMENT AND INPUTS
    // Script reference for Player Inputs
    private PlayerInputManager pInput;

    //Movements
    [Header("Movement Settings")]
    [Space(10)]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSmoothSpeed = 20f;


    // Start is called once before the first frame update.
    void Start()
    {
        // Get PlayerInputManager Script from GameObject.
        pInput = GetComponent<PlayerInputManager>();
        // Get Character Controller Component
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame.
    void Update()
    {
        // Creates a Vector 3 using our stored values from "OnMove" Function-
        // -in our Input System and applies them to the x and z.
        Vector3 targetDirection = new Vector3(pInput.move.x, 0, pInput.move.y);
        // Creates a Quaternion variable that makes the targetRotation the targetDirection Input.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        if(pInput.move != Vector2.zero)
        {
            // Changes the direction the character is facing and smooths the change.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }
        

        // Moves the controller based of the targetDirection values * player speed over time.
        controller.Move(targetDirection * speed * Time.deltaTime);
    }
}
