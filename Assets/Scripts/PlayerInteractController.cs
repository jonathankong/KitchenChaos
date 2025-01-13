using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private InteractionManager _interactManager;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private FloatReference _interactRange;
    private bool _isInRange = false;

    private Vector3 _playerVisualCenter;
    private Vector3 _inputMoveDirection;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _playerVisual = transform.GetChild(0);
    }

    private void Update()
    {
        _playerVisualCenter = _playerVisual.TransformPoint(_collider.center);

    }

    private void FixedUpdate()
    {
        _isInRange = _interactManager.CheckInRangeToInteract(_collider, _playerVisual, _interactRange);
    }

    private void OnMoveInput(Vector2 vector)
    {
        _inputMoveDirection = new Vector3(vector.x, 0f, vector.y);
    }

    private void OnDrawGizmos()
    {
        if (_isInRange) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        if (_playerVisual != null) 
            Gizmos.DrawLine(transform.TransformPoint(_collider.center), transform.TransformPoint(_collider.center) + _playerVisual.forward * _interactRange.Value);
    }

    private void OnInteract(bool isInteract)
    {
        if (isInteract && _isInRange) _interactManager.TriggerInteraction();
    }

    private void OnEnable()
    {
        _inputReader.MovePerformed += OnMoveInput;
        _inputReader.Interact += OnInteract;
    }

    private void OnDisable()
    {
        _inputReader.MovePerformed -= OnMoveInput;
        _inputReader.Interact -= OnInteract;
    }
}
