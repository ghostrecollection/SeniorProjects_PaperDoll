using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    // CHARACTER CONTROLLER
    private CharacterController controller;

    // MOVEMENT AND INPUTS
    // Script reference for Player Inputs
    private PlayerInputManager pInput;

    // MOVEMENT
    [Header("Movement Settings")]
    [Space(10)]
    [SerializeField] float currentSpeed;
    [SerializeField] float idleSpeed = 0f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    // JUMP
    [Space(10)]
    [SerializeField] float jumpHeight = 2f;

    [Header("Movement Transitions")]
    [Space(10)]
    //[SerializeField] float movementSmoothSpeed = 3f;
    [SerializeField] float rotationSmoothSpeed = 10f;

    [Space(30)]

    // GROUNDING
    [Header("Grounding Setup")]
    [Space(10)]
    [SerializeField] bool isGrounded;
    // Layer for ground detection.
    [SerializeField] LayerMask groundLayer;
    // Empty location at player's feet.
    [SerializeField] Transform groundCheck;
    [SerializeField] float gravity = -15f;
    private Vector3 velocity;

    [Space(30)]

    // CAMERA
    [Header("Camera Settings")]
    [Space(10)]
    [SerializeField] GameObject mainCam;
    [SerializeField] Transform cameraFollowTarget;
    private float xRotation;
    private float yRotation;

    // --- START ---
    void Start()
    {
        // Get PlayerInputManager Script from GameObject.
        pInput = GetComponent<PlayerInputManager>();
        // Get Character Controller Component
        controller = GetComponent<CharacterController>();
    }


    // --- UPDATE ---
    void Update()
    {
        // Debug.Log($"currentSpeed: {currentSpeed}");

        // Movement Function
        OnMove();

        // Jump and Gravity Function
        JumpAndGravity();
    }


    // --- LATE UPDATE --- 
    private void LateUpdate()
    {
        CameraSettings();
    }


    // --- CAMERA SETTINGS --- 
    void CameraSettings()
    {

        cameraFollowTarget.rotation = Quaternion.identity;
    }


    // --- MOVEMENT ---
    void OnMove()
    {
        currentSpeed = idleSpeed;
        
        // Creates a Vector 3 using our stored values from "OnMove" Function-
        // -in our Input System and applies them to the x and z.
        Vector3 inputDir = new Vector3(pInput.move.x, 0, pInput.move.y);
        // Target rotation -- player rotates to direction input
        float targetRotation = 0;

        if (pInput.move != Vector2.zero)
        {
            // Running
            if (pInput.run)
            {
                currentSpeed = runSpeed;
                //float movementSmoothedSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * movementSmoothSpeed);
                //currentSpeed = movementSmoothedSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
                //float movementSmoothedSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * movementSmoothSpeed);
                //currentSpeed = movementSmoothedSpeed;
            }


            // Creates a Quaternion variable that makes the targetRotation the targetDirection Input.
            targetRotation = Quaternion.LookRotation(inputDir).eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            // Changes the direction the character is facing and smooths the change.
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothSpeed * Time.deltaTime);

        }

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        // Moves the controller based of the targetDirection values * player speed over time.
        controller.Move(inputDir * currentSpeed * Time.deltaTime);
    }


    // --- JUMP AND GRAVITY ---
    void JumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .2f, groundLayer);
        
        if (isGrounded)
        {
            //velocity.y = 0f;
            if (pInput.jump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
                pInput.jump = false;
            }
        }
        else
        {
            // Gravity is applied when player is in the air.
            velocity.y += gravity * Time.deltaTime;
        }

        // Applies the vertical movement.
        controller.Move(velocity * Time.deltaTime);
    }

}
