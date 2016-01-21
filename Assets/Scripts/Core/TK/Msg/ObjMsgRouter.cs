using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMsgRouter{
    private delegate void RelayMsgHandler(object o);

    private static Dictionary<eCmd, RelayMsgHandler> mRelayMsgHandlers = new Dictionary<eCmd, RelayMsgHandler>();

    public static bool SendObjMsg(eCmd cmd,IBaseMsgData data) {
        GameObject receiver=TKObjManager.FindObject(data.Receiver);
        if (receiver != null) {
            BaseObj  baseObj= receiver.GetComponent<BaseObj>();
            if (baseObj != null) {
                baseObj.OnObjMsg(cmd, data);
                return true;
            }
        }
        return false;
    }
}
