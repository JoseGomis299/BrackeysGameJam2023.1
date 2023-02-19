using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDoor : MonoBehaviour
{
    [SerializeField] private string color;
    [SerializeField] private GameObject door;
    void Start()
    {
        if(PlayerPrefs.GetInt(color, 0) == 1) door.SetActive(true);
    }
}
