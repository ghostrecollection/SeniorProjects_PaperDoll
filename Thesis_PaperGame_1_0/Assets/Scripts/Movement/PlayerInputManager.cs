using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    // INPUTS
    [Header("Inputs")]
    [Space(10)]
    // Vector2 for storing inputs.
    public Vector2 move;
    // Bool to check for shift key.
    public bool run;
    // Bool to check for space key.
    public bool jump;

    // OnMove Function through Input System.
    void OnMove(InputValue value)
    {
        // Defines move when movement vectors are stored by pressing WASD.
        move = value.Get<Vector2>();
    }
    // OnRun Function through Input System.
    void OnRun(InputValue value)
    {
        // Defines run as when shift is held.
        run = value.isPressed;
    }

    // OnJump Function through Input System.
    void OnJump(InputValue value)
    {
        // Defines jump as when space is pressed.
        jump = value.isPressed;
    }
}
