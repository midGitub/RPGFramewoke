using UnityEngine;
using System.Collections;

public class WasherCommand : Command {
    private Washer washer;

    public WasherCommand(Washer washer)
    {
        this.washer = washer;
    }

    public void Execute() {
        washer.Action();       
    }
	
}
