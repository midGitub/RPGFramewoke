using UnityEngine;
using System.Collections;

public class Skill2Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public Obj_Character objCharacter;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("skill2", false);
	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime > 1)
        {
            animator.SetBool("skill2", false);
        }
	}

	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}
}
