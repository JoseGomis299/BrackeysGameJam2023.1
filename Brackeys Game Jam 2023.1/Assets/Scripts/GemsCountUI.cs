using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemsCountUI : MonoBehaviour
{
    private TMP_Text _text;
    public static GemsCountUI Instance;
    private void Awake()
    {
        Instance = this;
        _text = GetComponent<TMP_Text>();
        PlayerController.OnCollectGem += OnCollectGem;
    }

    public void OnCollectGem()
    {
        _text.text = "GEMS: " + GemManager.Instance.gemCount;
    }
}
