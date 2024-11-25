using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/Idle")]
public class IdleDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var psc = controller as PlayerStateController;
        return psc != null && psc.MoveDirection.magnitude <= psc.MinMoveMagThreshold.Value;
    }
}
