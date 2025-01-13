using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Action/Walking")]
public class WalkingAction : Action
{
    public override void Act(StateController controller)
    {
        if (controller is IHasAnimator animatorController)
        {
            animatorController.Animator.SetBool(PlayerAnimatorHashes.IsWalking, true);
        }
    }
}
