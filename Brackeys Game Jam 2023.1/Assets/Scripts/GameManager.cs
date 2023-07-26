using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Transform _currentCheckPoint;
    public static GameManager Instance;
    
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject pauseMenu;
    public bool paused { get; private set; }
    
    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string nextScene)
    {
        transition.SetBool("ChangeScene", true);
        StartCoroutine(_LoadScene(nextScene));
        SetTransitionChildren(false);
    }

    public void Teleport(Transform teleportedTransform)
    {
        transition.SetBool("ChangeScene", true);
        StartCoroutine(_Teleport(teleportedTransform));
    }
    
    private IEnumerator _LoadScene(string nextScene)
    {
        Time.timeScale = 1;
        var player = GameObject.FindWithTag("Player");
        if(player != null) player.GetComponent<PlayerController>().enabled = false;
        if(pauseMenu != null) pauseMenu.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync(nextScene);
        yield return null;
    }
    
    private IEnumerator _Teleport(Transform teleportedTransform)
    {
        yield return new WaitForSeconds(0.6f);
        teleportedTransform.GetComponent<PlayerController>()?.MakeImmune();
        teleportedTransform.position = _currentCheckPoint.position;
        transition.SetBool("ChangeScene", false);
    }

    public void SetCurrentCheckPoint(Transform value)
    {
        _currentCheckPoint = value;
    }

    public void PauseGame()
    {
        paused = true;
        GameObject.FindWithTag("Player").GetComponent<PlayerAnimationManager>().enabled = false;
        Time.timeScale = 0;
        SoundManager.Instance.StopMusic();
        pauseMenu.SetActive(true);
    }
    
    public void ResumeGame()
    {
        paused = false;
        GameObject.FindWithTag("Player").GetComponent<PlayerAnimationManager>().enabled = true;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SoundManager.Instance.ResumeMusic();
    }
    
    public void RestartGame()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetTransitionChildren(bool value)
    {
        for (int i = 0; i < transition.transform.childCount; i++)
        {
            transition.transform.GetChild(i).gameObject.SetActive(value);
        }
    }

    public void GoToFinal()
    {
        transition.SetBool("GoToFinal", true);
    }
}
