using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/Walk")]
public class WalkAction : Action
{
    public override void Act(StateController controller)
    {
        var psc = controller as PlayerStateController;
        psc.PlayerAnimator.SetBool(PlayerAnimatorHashes.IsWalking, true);
    }
}