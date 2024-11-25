using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader")]
public class InputReaderSO : ScriptableObject, PlayerControls.IGameplayActions
{
    #region Singleton
    //private static readonly Lazy<PlayerControls> _playerControls = new Lazy<PlayerControls>(() => new PlayerControls());

    //public static PlayerControls playerInputActions
    //{
    //    get
    //    {
    //        return _playerControls.Value;
    //    }
    //}
    #endregion

    private PlayerControls _input;

    public event Action<Vector2> MoveEvent;

    private void OnEnable()
    {
        Debug.Log("InputReader OnEnable");
        if (_input == null)
        {
            _input = new PlayerControls();
            _input.Gameplay.SetCallbacks(this);
        }
        _input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("InputReader OnDisable");
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
