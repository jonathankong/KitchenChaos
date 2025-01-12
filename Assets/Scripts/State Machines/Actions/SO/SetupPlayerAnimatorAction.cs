using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/SetupAnimator")]
public class SetupPlayerAnimatorAction : Action
{
    public RuntimeAnimatorController animatorController;
    public override void Act(StateController controller)
    {
        controller.gameObject.GetComponent<Animator>().runtimeAnimatorController = animatorController;
    }
}
