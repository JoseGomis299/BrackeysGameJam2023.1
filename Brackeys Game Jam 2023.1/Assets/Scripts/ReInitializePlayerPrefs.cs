using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReInitializePlayerPrefs : MonoBehaviour
{
    [SerializeField] private ColorsSaving _colorsSaving;
    public void ResetData()
    {
        PlayerPrefs.SetInt("Red", 0);
        PlayerPrefs.SetInt("Blue", 0);
        PlayerPrefs.SetInt("Yellow", 0);
        _colorsSaving.ChangeColor();
    }

}
