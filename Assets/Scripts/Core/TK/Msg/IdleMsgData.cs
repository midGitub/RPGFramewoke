using UnityEngine;
using System.Collections;

public class IdleMsgData : IBaseMsgData
{
    protected Vector3 mPosition;
    public Vector3 Position {
        get { return mPosition; }
        set { mPosition = value; }
    }

    protected Vector3 mDirection;
    public Vector3 Direction {
        get { return mDirection; }
        set { mDirection = value; }
    }
}
