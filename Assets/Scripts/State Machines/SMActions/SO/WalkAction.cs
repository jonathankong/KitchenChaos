using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/Walk")]
public class WalkAction : SMAction
{
    public override void Act(StateController controller)
    {
        var psc = controller as PlayerStateController;
        psc.PlayerAnimator.SetBool(PlayerAnimatorHashes.IsWalking, true);
    }
}