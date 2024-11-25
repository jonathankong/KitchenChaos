using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private InputReaderSO _inputReader;

    [SerializeField]
    private FloatReference _moveSpeed;
    [SerializeField]
    private FloatReference _rotationSpeed;
    [SerializeField]
    private FloatReference _minMoveMagnitude;

    private Vector3 _moveDirection;

    private void Update()
    {
        transform.position += _moveDirection * _moveSpeed.Value * Time.deltaTime;
        TurnPlayerToFaceMovementDirection();
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMoveInput;
    }

    private void OnMoveInput(Vector2 vector) => _moveDirection = new Vector3(vector.x, 0f, vector.y); 

    private void TurnPlayerToFaceMovementDirection()
    {
        if (_moveDirection.magnitude > _minMoveMagnitude.Value) // Check if the movement is significant enough
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);

            // Smoothly rotate towards the target rotation using spherical rotations
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.Value * Time.deltaTime);
        }
    }
}
