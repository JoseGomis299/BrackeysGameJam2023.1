using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorMode doorMode;
    private enum DoorMode
    {
        Teleport,
        ChangeScene
    }

    [Header("Teleport")] 
    [SerializeField] private Transform newPosition;
    
    [Header("ChangeScene")]
    [SerializeField] private string nextScene;
    [SerializeField] private Animator transition;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            transition.SetBool("ChangeScene", true);
            if (doorMode == DoorMode.ChangeScene)
            {
                StartCoroutine(LoadScene());
            }
            else
            {
                StartCoroutine(Teleport(col.transform));
            }
        
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync(nextScene);
        yield return null;
    }
    
    private IEnumerator Teleport(Transform colTransform)
    {
        yield return new WaitForSeconds(0.6f);
        colTransform.position = newPosition.position;
        transition.SetBool("ChangeScene", false);
    }
}
