using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Highlighting is done by switching the selected visual object on or off so it overlaps the current object and give the appearance of it being selected
/// </summary>
public class CounterController : MonoBehaviour, IHighlightable, IInteractable
{
    [SerializeField] private KitchenObject _kitchenObj;
    [SerializeField] private Transform _unhighlightTransform;
    [SerializeField] private InteractionManager _interactManager;
    [SerializeField] private bool _hasSpawnedObject = false;
    [SerializeField] private Transform _objTransform;
    [SerializeField] private BoxCollider _collider;

    [SerializeField] private Vector3 _objSpawnPoint;

    public bool IsHighlighted => throw new System.NotImplementedException();

    private void Awake()
    {
        _unhighlightTransform = transform.GetChild(1);
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _objSpawnPoint = new Vector3(_collider.center.x, _collider.bounds.size.y, _collider.center.z);
    }

    public void OnHighlight()
    {
        if (HasReferences()) _unhighlightTransform.gameObject.SetActive(true);
    }

    public void OnUnhighlight()
    {
        if (HasReferences()) _unhighlightTransform.gameObject.SetActive(false);
    }

    private bool HasReferences()
    {
        if (_unhighlightTransform == null)
        {
            Debug.Log("CounterMaterialHandler: No references to switch to");
            return false;
        }
        return true;
    }

    public void Interact()
    {
        if (_hasSpawnedObject)  
            DespawnObject(); 
        else
            SpawnObject();
    }

    private void DespawnObject()
    {
        //Fresh instantiation
        if (_objTransform != null)
        {
            _objTransform.gameObject.SetActive(false);
        }
        _hasSpawnedObject = false;
    }

    private void SpawnObject()
    { 
        //Fresh instantiation
        if (_objTransform == null)
        {
            _objTransform = Instantiate(_kitchenObj.prefab, transform.TransformPoint(_objSpawnPoint), transform.rotation, transform);
        }
        else
        {
            _objTransform.gameObject.SetActive(true);
        }
        _hasSpawnedObject = true;
    }
}
