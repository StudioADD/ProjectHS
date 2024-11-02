using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonClass<T>
{
    private static SingletonClass<T> _instance;

    public static SingletonClass<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SingletonClass<T>();
            }

            return _instance;
        }
    }
}

public class SingletonLazy<T> where T : SingletonLazy<T>, new()
{
    private static Lazy<T> lazyInstance = null;

    public static T Instance
    {
        get
        {
            if (lazyInstance == null || !lazyInstance.IsValueCreated)
            {
                var instance = new T();
                lazyInstance = new Lazy<T>(() => instance);
            }

            return lazyInstance.Value;
        }
    }
}