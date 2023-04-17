using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 持久性的单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class PersistentSigleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance==null)
        {

            Instance = this as T;

        }else if(Instance!=this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

    }
}
