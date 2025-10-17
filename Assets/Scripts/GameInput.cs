using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputSystem_Actions InputSystem_Actions;
    public event EventHandler OnPlayerAttack;
    private void Awake()
    {
        Instance = this;
        InputSystem_Actions = new InputSystem_Actions();
        InputSystem_Actions.Enable();
        InputSystem_Actions.Player.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started (InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
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
