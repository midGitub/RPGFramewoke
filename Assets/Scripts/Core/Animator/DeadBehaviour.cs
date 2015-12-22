using UnityEngine;
using System.Collections;

public class DeadBehaviour : StateMachineBehaviour
{
    [HideInInspector]
    public Obj_Character objCharacter;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("dead", false);
	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime > 1)
        {
            animator.SetBool("dead", false);
        }
	}

	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}
}
