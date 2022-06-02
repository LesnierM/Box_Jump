using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static T _instance;
    public bool DontDestroyOnLoad;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance != null && (_instance as Singleton<T>).DontDestroyOnLoad)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        // Check if there is another instance and destroy if there is.
        if (GameObject.FindObjectsOfType<T>().Length > 1)
            Destroy(gameObject);
    }
}
