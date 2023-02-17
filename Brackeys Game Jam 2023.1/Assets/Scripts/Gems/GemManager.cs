using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public int gemCount;
    public const int TotalGemsNumber = 6;
    
    public static GemManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}
