using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputSystem_Actions _InputSystem_Actions;
    public event EventHandler _OnPlayerAttack;
    private void Awake()
    {
        Instance = this;
        _InputSystem_Actions = new InputSystem_Actions();
        _InputSystem_Actions.Enable();
        _InputSystem_Actions.Player.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started (InputAction.CallbackContext obj)
    {
        _OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _InputSystem_Actions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
    public void DisableMovement()
    {
        _InputSystem_Actions.Disable();
    }
}
