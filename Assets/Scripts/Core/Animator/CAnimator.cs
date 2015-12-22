using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAnimator{
    private Animator mAnimator;
    private string curAniName;
    private Obj_Character objCharacter;
    private Dictionary<GameGlobalEnum.OBJ_BEHAVIOUR, StateMachineBehaviour> behaviours = new Dictionary<GameGlobalEnum.OBJ_BEHAVIOUR, StateMachineBehaviour>();

    public CAnimator(Animator animator,Obj_Character character) {
        mAnimator = animator;
        objCharacter = character;
        initBehaviour();
    }

    private void initBehaviour()
    {
        RunBehaviour runBehaviour = mAnimator.GetBehaviour<RunBehaviour>();
        Atk1Behaviour atk1Behaviour = mAnimator.GetBehaviour<Atk1Behaviour>();
        Atk2Behaviour atk2Behaviour = mAnimator.GetBehaviour<Atk2Behaviour>();
        Atk3Behaviour atk3Behaviour = mAnimator.GetBehaviour<Atk3Behaviour>();
        Atk4Behaviour atk4Behaviour = mAnimator.GetBehaviour<Atk4Behaviour>();
        DeadBehaviour deadBehaviour = mAnimator.GetBehaviour<DeadBehaviour>();
        HitBehaviour hitBehaviour = mAnimator.GetBehaviour<HitBehaviour>();
        RollBehaviour rollBehaviour = mAnimator.GetBehaviour<RollBehaviour>();
        Skill1Behaviour skill1Behaviour = mAnimator.GetBehaviour<Skill1Behaviour>();
        Skill2Behaviour skill2Behaviour = mAnimator.GetBehaviour<Skill2Behaviour>();
        IdleBehaviour idleBehaviour = mAnimator.GetBehaviour<IdleBehaviour>();

        runBehaviour.objCharacter = objCharacter;
        atk1Behaviour.objCharacter = objCharacter;
        atk2Behaviour.objCharacter = objCharacter;
        atk3Behaviour.objCharacter = objCharacter;
        atk4Behaviour.objCharacter = objCharacter;
        deadBehaviour.objCharacter = objCharacter;
        hitBehaviour.objCharacter = objCharacter;
        rollBehaviour.objCharacter = objCharacter;
        skill1Behaviour.objCharacter = objCharacter;
        skill2Behaviour.objCharacter = objCharacter;
        idleBehaviour.objCharacter = objCharacter;

        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.RUN, runBehaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.ATK1, atk1Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.ATK2, atk2Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.ATK3, atk3Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.ATK4, atk4Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.IDLE, idleBehaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.DEAD, deadBehaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.ROLL, rollBehaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.SKILL1, skill1Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.SKILL2, skill2Behaviour);
        behaviours.Add(GameGlobalEnum.OBJ_BEHAVIOUR.HIT, hitBehaviour);
    }

    public void PlayAction(string animName,float speed=1) {
        if (mAnimator == null || !mAnimator.enabled)
            return;
        //先停止之前的动画
        if (curAniName != animName && !string.IsNullOrEmpty(curAniName))
            mAnimator.SetBool(Animator.StringToHash(animName),false);
        //播放新的动画
        mAnimator.SetBool(Animator.StringToHash(animName), true);
        if (!mAnimator.GetBool(Animator.StringToHash(animName))) {
            Debug.LogError("Error!can not find objCharacter animation:" + animName);
            return;
        }
        mAnimator.speed = speed;
        curAniName = animName;
    }

    public void StopAnimation() {
        if (mAnimator == null || !mAnimator.enabled)
            return;
        mAnimator.SetBool(curAniName,false);
    }

    public StateMachineBehaviour GetBehaviour(GameGlobalEnum.OBJ_BEHAVIOUR behaviour) {
        if (!behaviours.ContainsKey(behaviour))
            return null;
        return behaviours[behaviour];
    }

    public bool Enabled {
        get { return mAnimator.enabled; }
        set { mAnimator.enabled = value; }
    }

    public string PlayerActionName {
        get { return curAniName; }
    }

    public Obj_Character ObjCharacter {
        get { return objCharacter; }
    }

    public Dictionary<GameGlobalEnum.OBJ_BEHAVIOUR, StateMachineBehaviour> Behaviours {
        get { return behaviours; }
    }

    public float Speed {
        get
        {
            if (null == mAnimator || !mAnimator.enabled) return 0f;

            return mAnimator.speed;
        }
        set
        {
            if (null == mAnimator || !mAnimator.enabled) return;

            mAnimator.speed = value;
        }
    }
	
}
