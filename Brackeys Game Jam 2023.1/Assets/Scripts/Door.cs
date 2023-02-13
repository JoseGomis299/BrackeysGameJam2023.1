using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorMode doorMode;
    [SerializeField] private int neededGems;
    private bool _goalReached;
    private enum DoorMode
    {
        Teleport,
        ChangeScene
    }

    [Header("Teleport")] 
    [SerializeField] private Transform newPosition;
    
    [Header("ChangeScene")]
    [SerializeField] private string nextScene;

    private void Awake()
    {
        PlayerController.OnCollectGem += OnCollectGem;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && _goalReached)
        {
            if (doorMode == DoorMode.ChangeScene)
            {
                GameManager.Instance.LoadScene(nextScene);
            }
            else
            {
                GameManager.Instance.SetCurrentCheckPoint(newPosition);
                GameManager.Instance.Teleport(col.transform);
            }
        
        }
    }

    private void OnCollectGem()
    {
        if (GemManager.Instance.gemCount >= neededGems) _goalReached = true;
    }

   
}
