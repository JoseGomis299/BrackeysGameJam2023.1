using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using TMPro;
using UnityEngine;

public class GemsCountUI : MonoBehaviour
{
    public static GemsCountUI Instance;
    private void Awake()
    {
        Instance = this;
        PlayerController.OnCollectGem += OnCollectGem;
    }

    public void OnCollectGem()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < GemManager.Instance.gemCount)
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }

        
    }
    
}
