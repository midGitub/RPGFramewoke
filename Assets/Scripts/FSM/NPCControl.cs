using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NPCControl : MonoBehaviour {
    public GameObject player;
    public Transform[] paths;
    private FSMSystem fsm;

    public void SetTransition(Transition t) {
        fsm.PerformTransition(t);
    }

	// Use this for initialization
	void Start () {
        MakeFSM();    
	}

    private void MakeFSM() {
        FollowPathState follow = new FollowPathState(paths);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);

        fsm = new FSMSystem();
        fsm.AddFSMState(follow);
        fsm.AddFSMState(chase);
    }

    void FixedUpdate() {
        fsm.CurState.Reason(player,gameObject);
        fsm.CurState.Act(player,gameObject);
    }
}
