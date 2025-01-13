using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Action/Idle")]
public class IdleAction : Action
{
    public override void Act(StateController controller)
    {
        if (controller is IHasAnimator animatorController)
        {
            animatorController.Animator.SetBool(PlayerAnimatorHashes.IsWalking, false); 
        }
    }
}
