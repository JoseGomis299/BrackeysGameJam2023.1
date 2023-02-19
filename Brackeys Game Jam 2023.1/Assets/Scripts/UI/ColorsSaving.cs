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
        ChangeColor();
    }

    public void ChangeColor()
    {
        _blue = transform.GetChild(2).GetComponent<Image>();
        _red = transform.GetChild(0).GetComponent<Image>();
        _yellow = transform.GetChild(1).GetComponent<Image>();
        
        if (IntToBool(PlayerPrefs.GetInt("Blue", 0)))
        {
            var animator =  _blue.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = false;
            var col = _blue.color;
            col.r = 0;
            col.g = 0;
            col.b = 1;
            _blue.color = col;
        }
        else
        {
            var animator =  _blue.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = true;
            var col = _blue.color;
            col.r = 1f/255*60;
            col.g = 1f/255*60;
            col.b = 1f/255*60;
            _blue.color = col;
        }
        
        
        if (IntToBool(PlayerPrefs.GetInt("Red", 0)))
        {
            var animator =  _red.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = false;
            var col = _red.color;
            col.r = 1;
            col.g = 0;
            col.b = 0;
            _red.color = col;
        }
        else
        {
            var animator =  _red.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = true;
            var col = _red.color;
            col.r = 1f/255*90;
            col.g = 1f/255*90;;
            col.b = 1f/255*90;;
            _red.color = col;
        }
        
        
        if (IntToBool(PlayerPrefs.GetInt("Yellow", 0)))
        {
            var animator =  _yellow.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = false;
            var col = _yellow.color;
            col.r = 1;
            col.g = 1;
            col.b = 0;
            _yellow.color = col;
        }
        else
        {
            var animator =  _yellow.gameObject.GetComponent<Animator>();
            if(animator != null) animator.enabled = true;
            var col = _yellow.color;
            col.r = 1f/255*120;
            col.g = 1f/255*120;
            col.b = 1f/255*120;
            _yellow.color = col;
        }
    }

    private bool IntToBool(int value)
    {
        return value == 1;
    }
}
