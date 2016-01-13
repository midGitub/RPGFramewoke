using UnityEngine;
using System.Collections;

public class FollowPathState : FSMState {
    private int currentWayPotin;
    private Transform[] wayPoints;
    public FollowPathState(Transform[] tf) {
        wayPoints = tf;
        currentWayPotin = 0;
        stateId = StateID.FollowingPath;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        if(Physics.Raycast(npc.transform.position,npc.transform.forward,out hit,15)){
            if (hit.transform.gameObject.tag == "Player") {
                npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
                Debug.Log("-----15");
            }
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        Rigidbody rigidbody = npc.transform.GetComponent<Rigidbody>();
        Vector3 vel = rigidbody.velocity;
        Vector3 moveDir = wayPoints[currentWayPotin].position - npc.transform.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPotin++;
            if (currentWayPotin >= wayPoints.Length)
            {
                currentWayPotin = 0;
            }
        }
        else
        {
            vel = moveDir.normalized * 10;

            // Rotate towards the waypoint
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                      Quaternion.LookRotation(moveDir),
                                                      5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        }
        // Apply the Velocity
        rigidbody.velocity = vel;
    }
	
}
