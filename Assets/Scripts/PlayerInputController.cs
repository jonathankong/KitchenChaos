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

    private CapsuleCollider _collider;
    private Rigidbody _rigidBody;

    private Vector3 _inputMoveDirection;
    private Vector3 _updatedMoveDirection;
    private Vector3 _lastPerformedMoveDirection;
    private Vector3 _prevNormal = Vector3.zero;

    private float _playerVisualHeight;
    private float _playerVisualRadius;

    private bool _isCollided;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        TurnPlayerToFaceMovementDirection();
        if (!_isCollided) MovePlayer();
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

    private void OnInteract(bool isInteract)
    {
        Debug.Log($"Interact Button {isInteract}");
    }

    private void OnMoveInput(Vector2 vector)
    {
        _inputMoveDirection = new Vector3(vector.x, 0f, vector.y);
        _lastPerformedMoveDirection = new Vector3(vector.x, 0f, vector.y);
    }

    private void MovePlayer()
    {
        _rigidBody.MovePosition(transform.position + _inputMoveDirection * _moveSpeed.Value * Time.deltaTime);
        //transform.position += _inputMoveDirection * _moveSpeed.Value * Time.deltaTime;
    }

    private void TurnPlayerToFaceMovementDirection()
    {
        if (_inputMoveDirection.magnitude > _minMoveMagnitude.Value) // Check if the movement is significant enough
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_inputMoveDirection);

            // Smoothly rotate towards the target rotation using spherical rotations
            _rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.Value * Time.deltaTime));
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.Value * Time.deltaTime);
        }
    }

    #region Collision detection and update movement
    private void OnCollisionEnter(Collision collision)
    {
        _isCollided = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        //For some reason, there is more than 1 contact but the other one is a duplicate so we'll only use the first contact point
        var contact = collision.GetContact(0);
        //Push player outside of obstacle collider
        _rigidBody.MovePosition(transform.position - (contact.normal * contact.separation * Time.deltaTime));
        
        //If player is trying to leave the collision
        if (Vector3.Dot(_inputMoveDirection, contact.normal) > 0)
        {
            _inputMoveDirection = _lastPerformedMoveDirection;
        }
        //Calculate movement along surface of obstacle
        //Check to see if there's only 1 component that's non-zero;
        else
        {
            Vector3 newNormal;

            bool hasMoreThanOneNonZero = (contact.normal.x != 0 ? 1 : 0) +
                     (contact.normal.y != 0 ? 1 : 0) +
                     (contact.normal.z != 0 ? 1 : 0) > 1;

            //Determine the largest component of the normal and make it the new normal
            if (hasMoreThanOneNonZero)
            {
                var components = new float[] { contact.normal.x, contact.normal.y, contact.normal.z };
                var largestComp = contact.normal.x;
                var largestCompInd = 0;
                for (int i = 1; i < 3; i++)
                {
                    if (Mathf.Abs(components[i]) > Mathf.Abs(largestComp))
                    {
                        largestComp = components[i];
                        largestCompInd = i;
                    }
                }

                if (largestCompInd == 0) newNormal = new Vector3(largestComp, 0, 0);
                else if (largestCompInd == 1) newNormal = new Vector3(0, largestComp, 0);
                else newNormal = new Vector3(0, 0, largestComp);

                Debug.Log($"New Normal {newNormal}");
                //There's a weird quirk where a collision calulation is done as the player is leaving the box collider from the corner but my code determines the player must be rounding the corner instead and so the player traverses the other side of the corner as if it's magnatized to it.  
                //To get around this, I have to check to see if the player is holding down diagonal movement (thus it's sliding along the side of the collider) and if the normal has changed drastically (ie. the player was sliding down the z direction of the box collider and then the next collision calculation, my code thinks the player is sliding positively along the x direction).
                //If the normal changed drastically, just use the previous normal
                var isDiagonalMovement = (_lastPerformedMoveDirection.x != 0 && _lastPerformedMoveDirection.z != 0);
                var hasNormalChanged = (_prevNormal != Vector3.zero) && (_prevNormal.x == 0 && newNormal.x != 0 || _prevNormal.x != 0 && newNormal.x == 0 || _prevNormal.z == 0 && newNormal.z != 0 || _prevNormal.z != 0 && newNormal.x == 0);

                if (isDiagonalMovement && hasNormalChanged)
                    newNormal = _prevNormal;
            }
            else newNormal = contact.normal;

            //Project movement on to obstacle based on normal
            _inputMoveDirection = Vector3.ProjectOnPlane(_lastPerformedMoveDirection, newNormal);
            
            //Update previous normal
            _prevNormal = newNormal;

        }
        //This will now dictate movement until player exits other collider
        MovePlayer();
    }

    private void OnCollisionExit(Collision collision)
    {
        _isCollided = false;
        //Revert movement from plane projection to the input direction
        //This is for the player sliding along the obstacle
        _inputMoveDirection = _lastPerformedMoveDirection;
        //Reset _prevNormal
        _prevNormal = Vector3.zero;
    }
    #endregion

    /// <summary>
    /// Calculates some details of the player such as height and radius
    /// </summary>

    #region Draw Gizmos
    #endregion
}
