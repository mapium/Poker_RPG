using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputSystem_Actions InputSystem_Actions;
    private void Awake()
    {
        Instance = this;
        InputSystem_Actions = new InputSystem_Actions();
        InputSystem_Actions.Enable();
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = InputSystem_Actions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
}
