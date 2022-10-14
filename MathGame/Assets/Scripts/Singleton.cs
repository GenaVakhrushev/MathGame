using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

            Debug.LogWarning("More than one " + instance.GetType().ToString());

            return;
        }

        instance = this as T;
    }
}
