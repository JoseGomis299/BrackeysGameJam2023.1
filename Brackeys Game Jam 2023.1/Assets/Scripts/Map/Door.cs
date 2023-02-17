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

    public static event Action<int, int> OnChangeRoom; 
    private enum DoorMode
    {
        Teleport,
        ChangeScene
    }

    [Header("Teleport")] 
    [SerializeField] private Transform newPosition;
    [SerializeField] private int currentRoomIndex;
    [SerializeField] private int nextRoomIndex;

    [Header("ChangeScene")]
    [SerializeField] private string nextScene;

    private void Awake()
    {
        PlayerController.OnCollectGem += OnCollectGem;
    }

    private void Start()
    {
        if (GemManager.Instance == null && PlayerPrefs.GetInt(nextScene) == 0) _goalReached = true;
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
                StartCoroutine(ChangeRoom());
            }
        
        }
    }

    private void OnEnable()
    {
        if (_goalReached && transform.GetChild(1).gameObject.activeInHierarchy) transform.GetChild(1).gameObject.SetActive(false);
    }

    private IEnumerator ChangeRoom()
    {
        yield return new WaitForSeconds(0.5f);
        OnChangeRoom?.Invoke(currentRoomIndex, nextRoomIndex);
    }
    private void OnCollectGem()
    {
        if (this != null && !_goalReached && GemManager.Instance.gemCount >= neededGems)
        {
            _goalReached = true;
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

   
}
