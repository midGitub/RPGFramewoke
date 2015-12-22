using UnityEngine;
using System.Collections;

public class BaseAttr{

    private float moveSpeed = 10f;
    private bool isDead;

    public bool IsDead {
        set { isDead = value; }
        get { return isDead; }
    }

    public float MoveSpeed {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
}
