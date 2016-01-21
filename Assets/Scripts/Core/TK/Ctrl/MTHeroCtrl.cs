using UnityEngine;
using System.Collections;

public class MTHeroCtrl : BaseCtrl {
    protected Vector3 mJoyDir;
    public Vector3 JoyDir {
        get { return mJoyDir; }
    }

    private float sendeMoveTimer;
    private float sendeMoveTimeMax = 0.3f;

    public override void Init(int id, string objectName)
    {
        base.Init(id, objectName);
        mJoyDir = Vector3.zero;
        mOwnerScript = mOwnerObj.GetComponent<MTHero>();
        RegisterStateHandler(eState.Idle, new StateFunc(IdleStateUpdate));
        RegisterStateHandler(eState.Move, new StateFunc(MoveStateUpdate));
    }

    #region StateFunc
    private void IdleStateUpdate(float delta) {
        if (IsValidJoy()) {
            SendObjMsgHelper.SendIdleMsg(mOwnerScript.Name, mOwnerScript.Name, mOwnerScript.Direction, mOwnerScript.transform.position);
        }
    }

    private void MoveStateUpdate(float delta) {
        if (IsValidJoy())
        {
            SendObjMsgHelper.SendMoveMsg(mOwnerScript.Name, mOwnerScript.Name, mJoyDir, mOwnerScript.transform.position);
        }else {
            SendObjMsgHelper.SendIdleMsg(mOwnerScript.Name, mOwnerScript.Name, mOwnerScript.Direction, mOwnerScript.transform.position);
        }
    }
    #endregion

    protected override void UpdateCtrl(float delta)
    {
        UpdateJoyStick(delta);
        base.UpdateCtrl(delta);
    }

    private void UpdateJoyStick(float delta) { 
    
    }

    public bool IsValidJoy() {
        return mJoyDir.magnitude > 0.5f;
    }
}
