using UnityEngine;
using System.Collections;

public class ChasePlayerState : FSMState {
    public ChasePlayerState() {
        stateId = StateID.ChasingPlayer;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 30) {
            npc.GetComponent<NPCControl>().SetTransition(Transition.LostPlayer);
            Debug.Log("-----30");
        }
            
    }

    public override void Act(GameObject player, GameObject npc)
    {
        Rigidbody rigidbody = npc.transform.GetComponent<Rigidbody>();
        Vector3 vel = rigidbody.velocity;
        Vector3 moveDir = player.transform.position - npc.transform.position;

        // Rotate towards the waypoint
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                  Quaternion.LookRotation(moveDir),
                                                  5 * Time.deltaTime);
        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        vel = moveDir.normalized * 10;

        // Apply the new Velocity
        rigidbody.velocity = vel;
    }
}
