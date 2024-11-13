using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private InputReaderSO _inputReader;

    public FloatReference moveSpeed;
    public FloatReference rotationSpeed;

    private Vector3 _lastPosition;
    private Vector3 _moveDirection;

    private void Update()
    {
        transform.position += _moveDirection * moveSpeed.Value * Time.deltaTime;
        TurnPlayerToFaceMovementDirection();
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += MovePlayer;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= MovePlayer;
    }

    private void MovePlayer(Vector2 vector) => _moveDirection = new Vector3(vector.x, 0f, vector.y);

    private void TurnPlayerToFaceMovementDirection()
    {
        // Calculate the direction of movement
        Vector3 movementDirection = transform.position - _lastPosition;
        if (movementDirection.magnitude > 0.01f) // Check if the movement is significant enough
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed.Value * Time.deltaTime);
        }

        // Update the previous position for the next frame
        _lastPosition = transform.position;
    }
}
