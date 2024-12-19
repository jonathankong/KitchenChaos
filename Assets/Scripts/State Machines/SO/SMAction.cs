using UnityEngine;

public abstract class SMAction : ScriptableObject 
{
    public abstract void Act(StateController controller);
}
