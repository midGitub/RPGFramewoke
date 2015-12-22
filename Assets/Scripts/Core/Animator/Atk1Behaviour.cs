using UnityEngine;
using System.Collections;

public class Atk1Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public Obj_Character objCharacter;
    private string stateName = "atk1";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        objCharacter.StartEffect();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Animator.StringToHash(stateName)))
        {
            animator.SetBool(stateName, false);  
        }
        objCharacter.StopEffect();
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 1f) {
            animator.SetBool(stateName, false);           
        }

    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
