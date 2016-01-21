using UnityEngine;
using System.Collections;

public static class SendObjMsgHelper{

    public static void SendIdleMsg(string receiver, string sender, Vector3 dir, Vector3 pos)
    {
        IdleMsgData data = new IdleMsgData
        {
            Cmd=eCmd.Idle,
            Receiver=receiver,
            Sender=sender,
            Direction=dir,
            Position=pos
        };
        ObjMsgRouter.SendObjMsg(eCmd.Idle,data);
    }

    public static void SendMoveMsg(string receiver, string sender, Vector3 dir, Vector3 pos)
    {
        MoveMsgData data = new MoveMsgData
        {
            Cmd = eCmd.Move,
            Receiver = receiver,
            Sender = sender,
            Direction = dir,
            Position = pos
        };
        ObjMsgRouter.SendObjMsg(eCmd.Move, data);
    }
}
