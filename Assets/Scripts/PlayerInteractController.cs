using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private FloatReference _selectDistance;

    private Vector3 _playerVisualCenter;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        _playerVisualCenter = transform.TransformPoint(_collider.center);
    }

    private void FixedUpdate()
    {
        Debug.Log(Physics.Raycast(_playerVisualCenter, transform.forward, out RaycastHit hitInfo, _selectDistance.Value));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_playerVisualCenter, _playerVisualCenter + transform.forward * _selectDistance.Value);
    }
}
