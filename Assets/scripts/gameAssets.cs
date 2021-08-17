using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameAssets : MonoBehaviour
{
    public static gameAssets Instance;

    public Sprite Head;
    public Sprite Body;
    public Sprite massGainer;
    public Sprite massBurner;
    public Sprite shield;

    private void Awake()
    {
        Instance = this;
    }
}