using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorsSaving : MonoBehaviour
{
    private Image _blue;
    private Image _red;
    private Image _yellow;

    private bool _r;
    private bool _y;
    private void Awake()
    {
        _blue = transform.GetChild(2).GetComponent<Image>();
        _red = transform.GetChild(0).GetComponent<Image>();
        _yellow = transform.GetChild(1).GetComponent<Image>();
        
        if (IntToBool(PlayerPrefs.GetInt("Blue", 0)))
        {
            _blue.gameObject.GetComponent<Animator>().enabled = false;
            var col = _blue.color;
            col.r = 0;
            col.g = 0;
            col.b = 255;
            _blue.color = col;
        }
        if (IntToBool(PlayerPrefs.GetInt("Red", 0)))
        {
            _red.gameObject.GetComponent<Animator>().enabled = false;
            var col = _red.color;
            col.r = 1;
            col.g = 0;
            col.b = 0;
            Debug.Log(col);

            _red.color = col;
        }
        if (IntToBool(PlayerPrefs.GetInt("Yellow", 0)))
        {
            _yellow.gameObject.GetComponent<Animator>().enabled = false;
            var col = _yellow.color;
            col.r = 255;
            col.g = 255;
            col.b = 0;
            _yellow.color = col;
        }
    }

    private bool IntToBool(int value)
    {
        return value == 1;
    }
}
