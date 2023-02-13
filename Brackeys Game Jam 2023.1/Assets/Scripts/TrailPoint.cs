using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrailPoint : MonoBehaviour
{
     public bool canNext;
    [HideInInspector] public bool startCount;
    public float startTime;
    
    [Header("Conditions")]
    public float timeToStay;

    private void Awake()
    {
        canNext = false; 
        startCount = false;
    }

    private void Update()
    {
        if (!startCount) return;
        if (Time.time - startTime >= timeToStay)
        {
            canNext = true;
        }
    }

}
