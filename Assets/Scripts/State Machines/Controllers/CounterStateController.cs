//using UnityEngine;

//public class CounterStateController : StateController, IInteractable
//{
//    public bool IsHighlighted { get; private set; }

//    public event System.Action OnInteractRange;
//    public event System.Action OnNotInteractRange;

//    public void Highlight()
//    {
//        IsHighlighted = true;
//    }

//    public void Unhighlight()
//    {
//        IsHighlighted = false;
//    }

//    public void InvokeEvents()
//    {
//        if (IsHighlighted) OnInteractRange?.Invoke();
//        else OnNotInteractRange?.Invoke();
//    }

//    public void Interact()
//    {
//        Debug.Log("Interact!");
//    }
//}
