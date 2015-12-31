using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingMediator : BaseMediator {
    private Slider slider;
    private Text text;

    protected override void AddEvent()
    {
        base.AddEvent();
        slider = transform.Find("Slider").GetComponent<Slider>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    private float current;

    public override void Update()
    {
        base.Update();
        current+=Time.deltaTime;
        slider.value = current / 10.0f;
        text.text = (current / 10.0f) * 100 + "%";
        if (current >= 10) {
            Close();
            current = 0;
            SingletonObject<LoginMediator>.getInstance().Open();
        }
    }
}
