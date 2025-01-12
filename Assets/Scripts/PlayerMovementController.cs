using NUnit;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private InputReader _inputReader;

    [SerializeField]
    private FloatReference _moveSpeed;
    [SerializeField]
    private FloatReference _rotationSpeed;
    [SerializeField]
    private FloatReference _minMoveMagnitude;

    [SerializeField] private LayerMask _checkCollisionsLM;

    [SerializeField] private Transform _playerVisual;
    [SerializeField] private BoxCollider _collider;


    private Vector3 _inputMoveDirection;
    private Vector3 _updatedMoveDirection;
    private Vector3 _unaffectedMoveDirection;
    private Vector3 _prevNormal = Vector3.zero;

    private float _playerVisualHeight;
    private float _playerVisualRadius;

    private bool _hasCollided;
    private Vector3 _contactNormal;

    private Vector3 _bottomPoint;
    private Vector3 _topPoint;
    private float _distFromPlayer = 5f;


    private Collider[] _colliders;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        TurnPlayerVisualToFaceMovementDirection();
        CheckCollisions();
        if (!_hasCollided) MovePlayer();
    }

    private void OnEnable()
    {
        _inputReader.MovePerformed += OnMoveInput;

    }

    private void OnDisable()
    {
        _inputReader.MovePerformed -= OnMoveInput;
    }

    private void OnMoveInput(Vector2 vector)
    {
        _inputMoveDirection = new Vector3(vector.x, 0f, vector.y);
        _unaffectedMoveDirection = new Vector3(vector.x, 0f, vector.y);
    }

    private void MovePlayer()
    {
        transform.position += _inputMoveDirection * _moveSpeed.Value * Time.deltaTime;
    }


    private void TurnPlayerVisualToFaceMovementDirection()
    {
        // Check if the movement is significant enough
        if (_inputMoveDirection.magnitude > _minMoveMagnitude.Value) 
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_inputMoveDirection);

            // Smoothly rotate towards the target rotation using spherical rotations
            _playerVisual.rotation = (Quaternion.Slerp(_playerVisual.rotation, targetRotation, _rotationSpeed.Value * Time.deltaTime));
        }
    }

    //Check ahead to see if collisions are happening
    private void CheckCollisions()
    {
        var _bounds = _collider.bounds;
        var avgContactNormal = Vector3.zero;
        var totalContactNormal = Vector3.zero; 

        //If no important collisions, exit
        if (!Physics.CheckBox(_bounds.center, _bounds.extents, Quaternion.identity, _checkCollisionsLM))
        {
            _inputMoveDirection = _unaffectedMoveDirection;
            return;
        }

        _colliders = Physics.OverlapBox(_bounds.center, _bounds.extents, Quaternion.identity, _checkCollisionsLM);

        //Debug.Log($"{_collider.name} {_collider.transform.position} {_collider.transform.rotation}");
        foreach (var collider in _colliders)
        {
            //Get point of collider closest to this collider's center in world space
            var closestPoint = collider.ClosestPoint(_bounds.center);
            //Move the point down so it's inline with this collider's center.
            closestPoint = new Vector3(closestPoint.x, _collider.center.y, closestPoint.z);

            avgContactNormal += (_bounds.center - closestPoint).normalized;

            //Debug.Log($"{_collider.name} {_collider.transform.position} {_collider.transform.rotation}");
            //Debug.Log($"{collider.name} {collider.transform.position} {collider.transform.rotation}");

            Physics.ComputePenetration(_collider, _collider.transform.position, _collider.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out Vector3 direction, out float distance);

            //There's a weird interaction when the player's collider contacts the positive z direction of an obstacle and ComputePenetration calculates a distance very large for some reason
            //This then pushes the player out too far so I set a limit here.
            if (distance > 0.5)
                continue;

            //Ensure player does not move vertically as a result of a calculation
            direction.y = 0;
            Debug.Log($"{collider.name} Dir: {direction} Dist: {distance} Normal: {direction * distance}");
            totalContactNormal += direction * distance;
        }

        Debug.Log($"Distance to push : {totalContactNormal}");
        transform.position += totalContactNormal;

        if (Vector3.Dot(_unaffectedMoveDirection, avgContactNormal) >= 0)
        {
            _inputMoveDirection = _unaffectedMoveDirection;
        }
        else
        {
            //Project movement onto plane with averageContactNormal
            Debug.Log($"Projection on plane: {Vector3.ProjectOnPlane(_unaffectedMoveDirection, avgContactNormal)}");
            _inputMoveDirection = Vector3.ProjectOnPlane(_unaffectedMoveDirection, avgContactNormal);
        }
    }

    private Vector3 ConvertVectorToDigitalNormalize2DMovement(Vector3 vector)
    {
        Vector3 newVector;

        var components = new float[] { vector.x, vector.y, vector.z };
        var largestComp = vector.x;
        var largestCompInd = 0;
        for (int i = 1; i < 3; i++)
        {
            if (Mathf.Abs(components[i]) > Mathf.Abs(largestComp))
            {
                largestComp = components[i];
                largestCompInd = i;
            }
        }

        if (largestCompInd == 0) newVector = new Vector3(largestComp, 0, 0);
        else if (largestCompInd == 1) newVector = new Vector3(0, largestComp, 0);
        else newVector = new Vector3(0, 0, largestComp);

        return newVector;
    }

    /// <summary>
    /// Calculates some details of the player such as height and radius
    /// </summary>

    #region Draw Gizmos
    private void OnDrawGizmos()
    {
        //if (_colliders == null) return;

        //Gizmos.color = UnityEngine.Color.blue;

        //foreach (var collider in _colliders) {
        //    var closestPoint = collider.ClosestPoint(transform.TransformPoint(_collider.center));
        //    Debug.Log($"Closest Point {closestPoint}");

        //    Gizmos.DrawLine(closestPoint, transform.TransformPoint(_collider.center));
        //}
    }
    #endregion
}
