using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obj_Character : Obj {
    private Animator objAnimator;
    private NavMeshAgent navAgent;
    private GameObject moveTarget;
    private BaseAttr baseAttr;
    private bool isMoving;

    protected Dictionary<GameGlobalEnum.OBJ_BEHAVIOUR, StateMachineBehaviour> behaviours = new Dictionary<GameGlobalEnum.OBJ_BEHAVIOUR, StateMachineBehaviour>();

    public Obj_Character() {
        mObjType = GameGlobalEnum.OBJ_TYPE.OBJ_CHARACTER;
        baseAttr = new BaseAttr();
    }

    protected override void Awake() {
        base.Awake();
        objAnimator=GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
        initBehaviour();
	}

    protected void initBehaviour()
    {
        RunBehaviour runBehaviour = objAnimator.GetBehaviour<RunBehaviour>();
        Atk1Behaviour atk1Behaviour = objAnimator.GetBehaviour<Atk1Behaviour>();
        Atk2Behaviour atk2Behaviour = objAnimator.GetBehaviour<Atk2Behaviour>();
        Atk3Behaviour atk3Behaviour = objAnimator.GetBehaviour<Atk3Behaviour>();
        Atk4Behaviour atk4Behaviour = objAnimator.GetBehaviour<Atk4Behaviour>();
        DeadBehaviour deadBehaviour = objAnimator.GetBehaviour<DeadBehaviour>();
        HitBehaviour hitBehaviour = objAnimator.GetBehaviour<HitBehaviour>();
        RollBehaviour rollBehaviour = objAnimator.GetBehaviour<RollBehaviour>();
        Skill1Behaviour skill1Behaviour = objAnimator.GetBehaviour<Skill1Behaviour>();
        Skill2Behaviour skill2Behaviour = objAnimator.GetBehaviour<Skill2Behaviour>();
        IdleBehaviour idleBehaviour = objAnimator.GetBehaviour<IdleBehaviour>();

        runBehaviour.objCharacter = this;
        atk1Behaviour.objCharacter = this;
        atk2Behaviour.objCharacter = this;
        atk3Behaviour.objCharacter = this;
        atk4Behaviour.objCharacter = this;
        deadBehaviour.objCharacter = this;
        hitBehaviour.objCharacter = this;
        rollBehaviour.objCharacter = this;
        skill1Behaviour.objCharacter = this;
        skill2Behaviour.objCharacter = this;
        idleBehaviour.objCharacter = this;
        
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

    protected void initNavAgent() {
        if (navAgent==null)
        {
            navAgent = GetComponent<NavMeshAgent>();
            if(navAgent==null){
                navAgent = gameObject.AddComponent<NavMeshAgent>();
            }
            navAgent.enabled = true;
            navAgent.speed = baseAttr.MoveSpeed;
            navAgent.radius = 0f;
            navAgent.acceleration = 10000f;
            navAgent.angularSpeed = 30000f;
            navAgent.autoRepath = false;
            navAgent.autoBraking = true;
        }
    }

    public void Idle() {
        objAnimator.Rebind();
    }

    public void Run() {
        objAnimator.SetBool("run", true);
    }

    public void Atk1() {
        objAnimator.SetBool("atk1", true);
    }

    public void Atk2()
    {
        objAnimator.SetBool("atk2", true);
    }

    public void Atk3()
    {
        objAnimator.SetBool("atk3", true);
    }

    public void Atk4()
    {

        objAnimator.SetBool("atk4", true);
    }

    public void Skill1()
    {
        objAnimator.SetBool("skill1", true);
    }

    public void Skill2()
    {
        objAnimator.SetBool("skill2", true);
    }

    public void Roll()
    {
        objAnimator.SetBool("roll",true);
    }

    public void Dead() {
        objAnimator.SetBool("dead", true);
    }



    public Animator ObjAnimator
    {
        get { return objAnimator; }
    }

    public NavMeshAgent ObjNavAgent
    {
        get { return navAgent; }
    }

    public GameObject MoveTarget
    {
        get { return moveTarget; }
        set { moveTarget = value; }
    }

    public BaseAttr Attribute
    {
        get { return baseAttr; }
        set { baseAttr = value; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public bool IsDie() {
        return baseAttr.IsDead;
    }
}
