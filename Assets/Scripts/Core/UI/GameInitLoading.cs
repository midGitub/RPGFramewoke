using UnityEngine;
using System.Collections;

public class GameInitLoading : BaseLoading
{
    public void StartLoading() { 
        LoadingManager.getInstance().CurrentLoading=this;
    }

    public void EndLoading() {
        LoadingManager.getInstance().CurrentLoading = null;
    }

    public override void Update()
    {
        base.Update();
        if(SingletonObject<LoadingMediator>.getInstance().IsOpen){
            SingletonObject<LoadingMediator>.getInstance().Progress = ResourceManager.currentCount/(float)ResourceManager.totalCount;
        }
        if (ResourceManager.currentCount / (float)ResourceManager.totalCount >= 1)
            LoadingCompleted();
    }

    protected override void LoadingCompleted()
    {
        base.LoadingCompleted();
        EndLoading();
        GameManager.getInstance().SetGameState(eGameState.Login);
    }
}
