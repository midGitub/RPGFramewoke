using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LoginMediator : BaseMediator {
    private GameObject loginBtn;

    protected override void AddEvent() {
       loginBtn = transform.Find("(Button)login").gameObject;
       EventTriggerListener.Get(loginBtn).PointerClick += OnClick;
    }

    void OnClick(PointerEventData eventData)
    {
        GLog.Log("login success!");
    }
}
