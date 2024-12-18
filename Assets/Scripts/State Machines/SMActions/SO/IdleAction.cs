using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/Idle")]
public class IdleAction : SMAction
{
    public override void Act(StateController controller)
    {
        var psc = controller as PlayerStateController;
        psc.PlayerAnimator.SetBool(PlayerAnimatorHashes.IsWalking, false);
    }
}
