using UnityEngine;
using System.Collections;

public class Obj : MonoBehaviour {

    protected GameGlobalEnum.OBJ_TYPE mObjType = GameGlobalEnum.OBJ_TYPE.OBJ;

    protected int objId;

    protected Transform objTransform;

    public GameGlobalEnum.OBJ_TYPE ObjType {
        get { return mObjType; }
    }

    public int ObjId {
        get { return objId; }
        set { objId = value; }
    }

    public Transform ObjTransform {
        get {
            return objTransform;
        }
    }

    public Vector3 Position {
        get { return objTransform.localPosition; }
        set { objTransform.localPosition = value; }
    }

    public Quaternion Rotation {
        get { return objTransform.localRotation; }
        set { objTransform.localRotation = value; }
    }

    public Vector3 Scale {
        get { return objTransform.localScale; }
        set { objTransform.localScale = value; }
    }

    protected virtual void Awake() {
        objTransform = transform;
    }

	// Use this for initialization
	void Start () {
	
	}
}
