using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingMediator : BaseMediator {
    private Slider slider;
    private Text text;
    private float progress;
    private GameObject loadingGame;

    public LoadingMediator() :
        base()
    {
        loadingGame = (GameObject)Resources.Load("panel_loading");
        if (loadingGame == null)
        {
            GLog.LogError("can't find the game object");
        }
        PanelName=loadingGame.name;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        slider = transform.Find("Slider").GetComponent<Slider>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    public override void Update()
    {
        base.Update();
        text.text = progress * 100 + "%";
        slider.value = progress;
        if (progress >= 1) {
            if (ResourceManager.currentCount >= ResourceManager.totalCount) {
                Close();
                progress = 0;
                ResourceManager.currentCount = 0;
                ResourceManager.totalCount = 0;
            }
        }
    }

    public float Progress {
        set { progress = value; }
    }

    public GameObject LoadingGame
    {
        get
        {
            if (loadingGame != null)
            {
                return loadingGame;
            }
            else
            {
                GLog.LogError("the loading game object is null");
                return null;
            }
        }
    }
}
