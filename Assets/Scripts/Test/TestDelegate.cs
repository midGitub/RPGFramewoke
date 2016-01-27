using UnityEngine;
using System.Collections;
using System;

public class TestDelegate : MonoBehaviour {
    public delegate void ADemo(int test);
    public delegate string BDemo(int test);

    ADemo ademo;
    BDemo bdemo;

    public Action A;
    public Action<int> B;
    public Action<int, string> C;

    public Func<int, int> E;
    public Func<int, int, string> F;
    public Func<int, string, string> G;

    void Start()
    {
        ademo += printYourWord;
        bdemo = returnYourWord;
        ademo(1);
        Debug.Log(bdemo(1));

        A = () => { Debug.Log("I'm A "); };
        B = (i) => { Debug.Log("I'm B " + i); };
        C = (i, s) => { Debug.Log("I'm C " + i + " " + s); };

        E = (tempf) => { return tempf + 1; };

        F = (temp1, temp2) => { return "result:"+(temp1+temp2); };

        G = testF;

        A();
        B(1);
        C(1, "a");

        Debug.Log(E(10));

        ObjectHandler handermanager = new ObjectHandler();
        handermanager.hand += handmethod;
        handermanager.hand("test");

        handermanager.handEvent += handmethod;
        //handermanager.handEvent("test1");//编译器会报错
        Debug.Log("-------" + F(1, 2));
    }

    public string testF(int i,string s) {
        return i + s;
    }

    public void handmethod(string eventstring)
    {
        Debug.Log("handmethod:" + eventstring);
    }

    public void printYourWord(int i)
    {
        Debug.Log("hi, i am delegate");
    }

    public string returnYourWord(int i)
    {
        return "hi, i am delegateB,i will return something";
    }
}
public delegate void MoveHandler(string handle);
public class ObjectHandler
{

    public MoveHandler hand;
    public event MoveHandler handEvent;
    public void Onhand()
    {
        hand("句柄事件");
    }

    public void onHandEvent()
    {
        handEvent("带Event关键字的句柄事件");
    }
}