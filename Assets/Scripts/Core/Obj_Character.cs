using UnityEngine;
using System.Collections;

public class Obj_Character : Obj {
    private Animator objAnimator;
    private NavMeshAgent navAgent;
    private GameObject moveTarget;
    private BaseAttr baseAttr;
    private bool isMoving;

    public Obj_Character() {
        mObjType = GameGlobalEnum.OBJ_TYPE.OBJ_CHARACTER;
        baseAttr = new BaseAttr();
    }

    public Animator ObjAnimator {
        get { return objAnimator; }
    }

    public NavMeshAgent ObjNavAgent {
        get { return navAgent; }
    }

    public GameObject MoveTarget {
        get { return moveTarget; }
        set { moveTarget = value; }
    }

    public BaseAttr Attribute {
        get { return baseAttr; }
        set { baseAttr = value; }
    }

    public bool IsMoving {
        get { return isMoving; }
        set { isMoving = value; }
    }

	// Use this for initialization
	void Start () {
        init();
	}

    private void init() {
        
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

    public bool IsDie() {
        return baseAttr.IsDead;
    }
}
