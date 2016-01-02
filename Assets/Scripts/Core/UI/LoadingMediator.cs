using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingMediator : BaseMediator {
    private Slider slider;
    private Text text;
    private float progress;

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
        GLog.Log("progress-------"+progress);
        if (progress >= 1) {
            Close();
            progress = 0;
        }
    }

    public float Progress {
        set { progress = value; }
    }
}
