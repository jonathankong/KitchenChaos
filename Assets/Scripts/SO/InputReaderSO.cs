using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader")]
public class InputReaderSO : ScriptableObject, PlayerControls.IGameplayActions
{
    private PlayerControls _input;

    public event Action<Vector2> MoveEvent;

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new PlayerControls();
            _input.Gameplay.SetCallbacks(this);
        }
        _input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Gameplay.Disable();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
