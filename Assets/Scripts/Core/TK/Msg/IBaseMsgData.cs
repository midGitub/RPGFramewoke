using UnityEngine;
using System.Collections;

public class IBaseMsgData{
    protected eCmd mCmd;
    protected string mReceiver;
    protected string mSender;

    public eCmd Cmd {
        get { return mCmd; }
        set { mCmd = value; }
    }

    public string Receiver {
        get { return mReceiver; }
        set { mReceiver = value; }
    }

    public string Sender {
        get { return mSender; }
        set { mSender = value; }
    }
}
