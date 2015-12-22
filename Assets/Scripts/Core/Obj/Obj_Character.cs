using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obj_Character : Obj {
    protected CAnimator objAnimator;
    private NavMeshAgent navAgent;
    private GameObject moveTarget;
    private BaseAttr baseAttr;
    private bool isMoving;

    public int a;

    public Obj_Character() {
        mObjType = GameGlobalEnum.OBJ_TYPE.OBJ_CHARACTER;
        baseAttr = new BaseAttr();
    }

    protected override void Awake() {
        base.Awake();
        objAnimator=new CAnimator(GetComponent<Animator>(),this);
    }

	void Start () {
        
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

    public void PlayAnima(string name) {
        objAnimator.PlayAction(name);
    }

    public void StopAnim() {
        objAnimator.StopAnimation();
    }

    public void StartEffect() { 
    
    }

    public void StopEffect() { 
    
    }

    public CAnimator ObjAnimator
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
