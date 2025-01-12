using UnityEngine;

[CreateAssetMenu(fileName = "InteractionManager", menuName = "Managers/InteractionManager")]
public class InteractionManager : ScriptableObject
{
    // Event or methods to handle interactions
    public delegate void InteractionEvent(GameObject interactedObject);
    public event InteractionEvent OnObjectInteracted;

    private IHighlightable _prevHighlightObj;

    #region Debug Gizmo Information
    private Transform _t;
    private FloatReference _fr;
    private bool _range = false;
    #endregion
    public void TriggerInteraction(GameObject obj)
    {
        // Trigger interaction logic, like highlighting objects
        OnObjectInteracted?.Invoke(obj);
    }

    public bool CheckInRangeToInteract(Transform objTransform, FloatReference interactRange)
    {
        _t = objTransform;
        _fr = interactRange;
        var inRangeToInteract = Physics.Raycast(objTransform.position, objTransform.forward, out RaycastHit hitInfo, interactRange.Value);
        _range = inRangeToInteract;

        if (inRangeToInteract && hitInfo.collider.TryGetComponent(out IHighlightable highlightObj))
        { 
            //Unhighlight the previous object that was highlighted
            if (_prevHighlightObj != null) _prevHighlightObj.OnUnhighlight();
            highlightObj.OnHighlight();
            _prevHighlightObj = highlightObj;
            return true;
        }
        else
        {
            if (_prevHighlightObj != null) _prevHighlightObj.OnUnhighlight();
            _prevHighlightObj = null;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_range) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawLine(_t.position + Vector3.up, (_t.position + Vector3.up) + _t.forward * _fr.Value);
    }
}

