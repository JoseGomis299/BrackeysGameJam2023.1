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
   private Transform ballPos;
   [SerializeField] private GameObject gemBag;

   [Header("Gem")]
   [SerializeField] private char color;

   [SerializeField] private AudioClip particleSound;
   [SerializeField] private float particleSoundCooldown;
   private float _lastParticleSound;

   [Header("CameraShake")] 
   [SerializeField] private float magnitude;

   private CinemachineBasicMultiChannelPerlin noise;
   private CinemachineVirtualCamera cam;

   public static event Action<char> OnAltarGot;

   private void Awake()
   {
      ballPos = transform.GetChild(1);
      playerTransform = GameObject.FindWithTag("Player").transform;
      _canvas = transform.GetChild(0).gameObject;
      cam = GameObject.FindWithTag("CVcam").GetComponent<CinemachineVirtualCamera>();
      noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
   }
   
   public bool DisplayButton()
   {
      if(doNotInteract) return false;
      _canvas.SetActive(!(Vector3.Distance(playerTransform.position, transform.position) > radius));
      return _canvas.activeInHierarchy;
   }
   public void StartInteraction()
   {
      if(doNotInteract) return;
      gemBag.transform.position =ballPos.position;
      gemBag.GetComponent<GemBag>().DontGo = true;
      gemBag.GetComponent<SineMovement>().initialPos =ballPos.position;
      gemBag.gameObject.SetActive(true);
      _lastTimePressed = Time.time;
   }
   public void Interact()
   {
      if(doNotInteract) return;
      if (_canvas.activeInHierarchy) _canvas.SetActive(false);
      if (Time.time-_lastTimePressed < pressTime)
      {
         Noise(1,magnitude);
      }
      if (Time.time - _lastTimePressed >= pressTime)
      {
         Noise(0,0);
         OnAltarGot?.Invoke(color);
         gemBag.gameObject.SetActive(false);
         doNotInteract = true;
         Invoke(nameof(returnToLobby), 1.5f);
         StartCoroutine(PlaySounds());
      }
   }
   public void EndInteraction()
   {
      Noise(0,0);
   }

   private IEnumerator PlaySounds()
   {
      yield return new WaitForSeconds(0.65f);
      while (true)
      {
         if (Time.time - _lastParticleSound > particleSoundCooldown)
         {
            _lastParticleSound = Time.time;
            SoundManager.Instance.PlaySound(particleSound);
         }

         yield return null;
      }
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
