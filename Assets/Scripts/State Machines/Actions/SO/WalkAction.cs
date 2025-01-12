using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/Walk")]
public class WalkAction : Action
{
    public override void Act(StateController controller)
    {
        if (controller is IHasAnimator animatableController)
            animatableController.Animator.SetBool(PlayerAnimatorHashes.IsWalking, true);
    }
}