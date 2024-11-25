using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/Walk")]
public class WalkingDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var psc = controller as PlayerStateController;
        return psc != null && psc.MoveDirection.magnitude > psc.MinMoveMagThreshold.Value;
    }
}
