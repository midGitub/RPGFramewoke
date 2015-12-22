using System;
using System.Collections.Generic;

public class SingletonObject<T>
{
    private static T mSingleton;

    public static T getInstance()
    {
        if (mSingleton == null)
        {
            mSingleton = (T)Activator.CreateInstance(typeof(T));
        }
        return mSingleton;
    }
}