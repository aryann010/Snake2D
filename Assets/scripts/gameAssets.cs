using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameAssets : MonoBehaviour
{
    private static gameAssets instance;
    public static gameAssets Instance { get { return instance; } }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite head;
    public Sprite massGainer, body;
}
