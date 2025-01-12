using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader")]
public class InputReader : ScriptableObject, PlayerControls.IGameplayActions, PlayerControls.IDebugActions, PlayerControls.IDebugInGameplayActions
{
    private PlayerControls _input;


    public event Action<Vector2> MovePerformed;

    public event System.Action ToggleDebugMenu;
    public event Action<bool> DebugMouseSelect;

    public event Action<bool> Interact;
    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new PlayerControls();
            _input.Gameplay.SetCallbacks(this);

#if UNITY_EDITOR && DEBUG
            _input.DebugInGameplay.SetCallbacks(this);
#endif
        }
        _input.Gameplay.Enable();

#if UNITY_EDITOR && DEBUG
        _input.DebugInGameplay.Enable();
#endif
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Gameplay.Disable();

#if UNITY_EDITOR && DEBUG
            _input.DebugInGameplay.Disable(); 
#endif
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) MovePerformed?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMovementPerformed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) MovePerformed?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) Interact?.Invoke(context.ReadValueAsButton());
    }

    #region Debug Menu
    public void OnDebugToggle(InputAction.CallbackContext context)
    {
        ToggleDebugMenu?.Invoke();
    }

    public void OnMouseSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled) DebugMouseSelect?.Invoke(true);
    }
    #endregion
}
