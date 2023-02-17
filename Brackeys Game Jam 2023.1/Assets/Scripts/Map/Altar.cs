using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Altar : MonoBehaviour, Iinteractable
{
   [Header("Altar")] 
   [SerializeField] private float pressTime;
   [SerializeField] private float radius;
   private float _lastTimePressed;
   private bool doNotInteract;
   private Transform playerTransform;
   private GameObject _canvas;

   [Header("Gem")]
   [SerializeField] private char color;

   [Header("CameraShake")] 
   [SerializeField] private float magnitude;

   private CinemachineBasicMultiChannelPerlin noise;
   private CinemachineVirtualCamera cam;

   public static event Action<char> OnAltarGot;

   private void Awake()
   {
      playerTransform = GameObject.FindWithTag("Player").transform;
      _canvas = transform.GetChild(0).gameObject;
      cam = GameObject.FindWithTag("CVcam").GetComponent<CinemachineVirtualCamera>();
      noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
   }
   
   public bool DisplayButton()
   {
      _canvas.SetActive(!(Vector3.Distance(playerTransform.position, transform.position) > radius));
      return _canvas.activeInHierarchy;
   }
   public void StartInteraction()
   {
      if(doNotInteract) return;
      _lastTimePressed = Time.time;
   }
   public void Interact()
   {
      if(doNotInteract) return;

      if (Time.time-_lastTimePressed < pressTime)
      {
         Noise(1,magnitude);
      }
      if (Time.time - _lastTimePressed >= pressTime)
      {
         Noise(0,0);
         OnAltarGot?.Invoke(color);
         doNotInteract = true;
      }
   }
   public void EndInteraction()
   {
      Noise(0,0);
      if(doNotInteract) Invoke(nameof(returnToLobby), 1f);
   }

   private void returnToLobby()
   {
      GameManager.Instance.LoadScene("Lobby");
   }
   
   private void Noise(float amplitudeGain, float frequencyGain) {
      noise.m_AmplitudeGain = amplitudeGain;
      noise.m_FrequencyGain = frequencyGain;    
   }
}
