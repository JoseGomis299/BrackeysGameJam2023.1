using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReInitializePlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.SetInt("Red", 0);
        PlayerPrefs.SetInt("Blue", 0);
        PlayerPrefs.SetInt("Yellow", 0);

    }

}
