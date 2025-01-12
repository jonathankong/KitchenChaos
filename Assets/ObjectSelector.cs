using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Canvas _debugMenu;

    private void OnEnable()
    {
        _inputReader.ToggleDebugMenu += ToggleDebugMenu;
    }

    private void OnDisable()
    {
        _inputReader.ToggleDebugMenu -= ToggleDebugMenu;
    }

    private void ToggleDebugMenu()
    {
        if (_debugMenu.gameObject.activeSelf)
            _debugMenu.gameObject.SetActive(false);
        else 
            _debugMenu.gameObject.SetActive(true);
     }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
