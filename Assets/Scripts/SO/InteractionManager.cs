using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "InteractionManager", menuName = "Managers/InteractionManager")]
public class InteractionManager : ScriptableObject
{
    // Event or methods to handle interactions
    public delegate void InteractionEvent(GameObject interactedObject);
    public event System.Action OnObjectInteracted;

    private IInteractable _currInteractObject;
    private IHighlightable _prevHighlightObj;

    public void TriggerInteraction()
    {
        if (_currInteractObject != null) _currInteractObject.Interact();
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

            _currInteractObject = hitInfo.collider.GetComponent<IInteractable>();

            return true;
        }
        else
        {
            if (_prevHighlightObj != null) _prevHighlightObj.OnUnhighlight();
            _prevHighlightObj = null;

            _currInteractObject = null;
        }
        return false;
    }
}

