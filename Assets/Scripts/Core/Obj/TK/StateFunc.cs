using UnityEngine;
using System.Collections;

public class StateFunc{
    public delegate void EnterFunc(StateParam param);

    public delegate void UpdateFunc(float delta);

    public delegate void ExitFunc();

    public EnterFunc enterFunc;

    public UpdateFunc updateFunc;

    public ExitFunc exitFunc;
	
}
