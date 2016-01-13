using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMSystem{
    private List<FSMState> states;
    private StateID curStateId;
    public StateID CurStateId {
        get { return curStateId; }
    }

    private FSMState curState;
    public FSMState CurState {
        get { return curState; }
    }

    public FSMSystem() {
        states = new List<FSMState>();
    }

    public void AddFSMState(FSMState state) {
        if (state == null) {
            Debug.LogError("state is null!");
            return;
        }
        if (states.Count == 0) {
            states.Add(state);
            curState = state;
            curStateId = state.StateId;
            return;
        }
        if (states.Find(x => x.StateId == state.StateId) != null) {
            Debug.LogError("this state has already exist!");
            return;
        }
        states.Add(state);    
    }

    public void DeleteState(StateID id) {
        if (StateID.NullStateID == id) {
            Debug.LogError("NullStateId is't allowed!");
            return;
        }
        FSMState state=states.Find(x=>x.StateId==id);
        if (state != null)
        {
            states.Remove(state);
        }
        else {
            Debug.LogError("this id wasn't on this list!");
        }
    }

    public void PerformTransition(Transition trans) {
        if (Transition.NullTransition == trans) {
            Debug.LogError("NullTransition is't allowed!");
            return;
        }
        
        StateID id=curState.GetState(trans);
        if (StateID.NullStateID == id)
        {
            Debug.LogError("curretn stateId is NullStateId");
            return;
        }     
        FSMState state=states.Find(x=>x.StateId==curStateId);
        if(state!=null){
            curStateId = id;
            curState.DobeforeLeaving();
            curState = state;
            curState.DobeforeEntering();
        }
    }
	
}


public enum Transition { 
    NullTransition=0,
    SawPlayer,
    LostPlayer,
}

public enum StateID { 
    NullStateID=0,
    FollowingPath,
    ChasingPlayer,
}

public abstract class FSMState {
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateId;

    public StateID StateId {
        get { return stateId; }
    }

    public void AddTransition(Transition trans,StateID id) {
        if (trans == Transition.NullTransition) {
            Debug.LogError("trans is Null!");
            return;
        }
        if (id == StateID.NullStateID) {
            Debug.LogError("id is Null!");
            return;
        }
        if (map.ContainsKey(trans)) {
            Debug.LogError("trans has already exist!");
            return;
        }
        map.Add(trans,id);
    }

    public void DeleteTransition(Transition trans) {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("trans is Null!");
            return;
        }
        if (!map.ContainsKey(trans)) {
            Debug.LogError("trans doesn't exist!");
            return;
        }
        map.Remove(trans);
    }

    public StateID GetState(Transition trans)
    {
        if (!map.ContainsKey(trans))
            return StateID.NullStateID;
        return map[trans];
    }

    public virtual void DobeforeEntering() { }

    public virtual void DobeforeLeaving() { }

    public abstract void Reason(GameObject player,GameObject npc);

    public abstract void Act(GameObject player, GameObject npc);
}