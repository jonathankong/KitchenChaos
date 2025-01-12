using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Highlighting is done by switching the selected visual object on or off so it overlaps the current object and give the appearance of it being selected
/// </summary>
public class CounterMaterialHandler : MonoBehaviour, IHighlightable
{
    [SerializeField] private Transform _unhighlightTransform;

    public bool IsHighlighted => throw new System.NotImplementedException();

    private void Awake()
    {
        _unhighlightTransform = transform.GetChild(1);
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

    //private void OnEnable()
    //{
    //    if (TryGetComponent(out CounterStateController counterStateController))
    //    {
    //        counterStateController.OnInteractRange += OnHighlight;
    //        counterStateController.OnNotInteractRange += OnUnhighlight;
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (TryGetComponent(out CounterStateController counterStateController))
    //    {
    //        counterStateController.OnInteractRange -= OnHighlight;
    //        counterStateController.OnNotInteractRange -= OnUnhighlight;
    //    }
    //}
}
