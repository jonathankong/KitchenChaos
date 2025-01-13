using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionManager", menuName = "Managers/InteractionManager")]
public class InteractionManager : ScriptableObject
{
    // Event or methods to handle interactions
    public delegate void InteractionEvent(GameObject interactedObject);
    public event InteractionEvent OnObjectInteracted;

    private IHighlightable _prevHighlightObj;

    public void TriggerInteraction(GameObject obj)
    {
        // Trigger interaction logic, like highlighting objects
        OnObjectInteracted?.Invoke(obj);
    }

    public bool CheckInRangeToInteract(BoxCollider objCollider, Transform objTransform, FloatReference interactRange)
    {
        var inRangeToInteract = Physics.Raycast(objTransform.TransformPoint(objCollider.center), objTransform.forward, out RaycastHit hitInfo, interactRange.Value);

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
}

