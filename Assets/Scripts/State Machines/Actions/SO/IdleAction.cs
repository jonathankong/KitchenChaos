using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/Idle")]
public class IdleAction : Action
{ 
    public override void Act(StateController controller)
    {
        if (controller is IHasAnimator animatableController)
            animatableController.Animator.SetBool(PlayerAnimatorHashes.IsWalking, false);
    }
}
