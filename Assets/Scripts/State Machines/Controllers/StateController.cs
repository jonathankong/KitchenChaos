using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    public State startState;
    public State previousState;
    public State currentState;
    public State remainState;
    public bool transitionStateChanged = false;
    [HideInInspector] public float stateTimeElapsed;

    public bool isActive = true;

    public virtual void Start()
    {
        OnSetupState(); // setup when game starts
    }

    public virtual void OnSetupState()
    {
        if (currentState)
            currentState.DoSetupActions(this);
    }

    public virtual void OnExitState()
    {
        // reset time in this state
        stateTimeElapsed = 0;
        if (currentState)
            currentState.DoExitActions(this);
    }

    // for visual aid to indicate which state this object is currently at
    //public virtual void OnDrawGizmos()
    //{
    //    if (currentState != null)
    //    {
    //        Gizmos.color = currentState.sceneGizmoColor;
    //        Gizmos.DrawWireSphere(this.transform.position, 1.0f);
    //    }
    //}

    /********************************/
    // Regular methods
    // no action should be done here, strictly for transition
    public void TransitionToState(State nextState)
    {
        if (nextState == remainState) return;

        // The following two methods only happens once if nextState != remainstate
        OnExitState(); // cast exit action if any,

        // transition the states
        previousState = currentState;
        currentState = nextState;
        transitionStateChanged = true;

        OnSetupState(); // cast entry action if any
    }

    public void Update()
    {
        if (!isActive) return; // this is different from gameObject active, allow for separate control

        currentState.UpdateState(this);
    }
    /********************************/




}
