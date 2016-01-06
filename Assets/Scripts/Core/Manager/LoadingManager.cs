using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingManager : SingletonObject<LoadingManager>
{
    private BaseLoading curretnLoading;

    public BaseLoading CurrentLoading {
        set { curretnLoading = value; }
    }

    public	void Update () {
        if (curretnLoading != null)
            curretnLoading.Update();
	}
}
