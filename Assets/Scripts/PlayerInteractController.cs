using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private InteractionManager _interactManager;

    [SerializeField] private FloatReference _interactRange;

    private BoxCollider _collider;

    private Vector3 _playerVisualCenter;

    private bool _isInInteractRange = false;

    private IHighlightable _prevHighlightObj;

    private RaycastHit _raycastHit;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _playerVisualCenter = transform.TransformPoint(_collider.bounds.center);
    }

    private void FixedUpdate()
    {
        _isInInteractRange = CheckInRangeToInteract();
    }

    private void PerformInteraction(bool isInteractBtnPressed)
    {
        if (isInteractBtnPressed && _isInInteractRange)
        {
            _raycastHit.collider?.GetComponent<IInteractable>().Interact();
            if (_raycastHit.collider == null) Debug.Log("Object doesn't have a collider");
        }
    }
    private bool CheckInRangeToInteract()
    {
        return _interactManager.CheckInRangeToInteract(transform, _interactRange);
    }

    private void OnEnable()
    {
        _inputReader.Interact += PerformInteraction;
    }

    private void OnDisable()
    {
        _inputReader.Interact -= PerformInteraction;
    }
}
