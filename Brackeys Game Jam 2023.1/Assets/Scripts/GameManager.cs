using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Transform _currentCheckPoint;
    public static GameManager Instance;
    
    [SerializeField] private Animator transition;
    
    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string nextScene)
    {
        transition.SetBool("ChangeScene", true);
        StartCoroutine(_LoadScene(nextScene));
    }

    public void Teleport(Transform teleportedTransform)
    {
        transition.SetBool("ChangeScene", true);
        StartCoroutine(_Teleport(teleportedTransform));
    }
    
    private IEnumerator _LoadScene(string nextScene)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync(nextScene);
        yield return null;
    }
    
    private IEnumerator _Teleport(Transform teleportedTransform)
    {
        yield return new WaitForSeconds(0.6f);
        teleportedTransform.position = _currentCheckPoint.position;
        transition.SetBool("ChangeScene", false);
    }

    public void SetCurrentCheckPoint(Transform value)
    {
        _currentCheckPoint = value;
    }
}
