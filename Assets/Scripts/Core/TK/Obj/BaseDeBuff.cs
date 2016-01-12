using UnityEngine;
using System.Collections;

public class BaseDeBuff{
    protected BaseCharacter mOwner;

    protected DeBuffType mType;
    public DeBuffType Type
    {
        get { return mType; }
    }

    public float mTime;

    protected float mValue;
    public float Value
    {
        get { return mValue; }
    }

    public virtual void Init(DeBuffType type, BaseCharacter owner, float time, float value)
    {
        mType = type;
        mOwner = owner;
        mTime = time;
        mValue = value;
    }

    public static BaseDeBuff getInstance(DeBuffType type)
    {
        return new BaseDeBuff();
    }

    public virtual void OnDestroy() { }

    public virtual void OnUpdate(float delta) { }
}
