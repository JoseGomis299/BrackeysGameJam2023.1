using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LobbyAltar : MonoBehaviour, Iinteractable
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
   [SerializeField] private AudioClip explosionSound;
   [SerializeField] private AudioClip endMusic;
   
   [Header("CameraShake")] 
   [SerializeField] private float magnitude;

   [SerializeField] private AudioClip earthSound;
   
   private CinemachineBasicMultiChannelPerlin noise;
   private CinemachineVirtualCamera cam;
      private void Awake()
      {
         ballPos = transform.GetChild(1);
         playerTransform = GameObject.FindWithTag("Player").transform;
         _canvas = transform.GetChild(0).gameObject;
         cam = GameObject.FindWithTag("CVcam").GetComponent<CinemachineVirtualCamera>();
         noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
         doNotInteract = (PlayerPrefs.GetInt("Red", 0) == 0 || PlayerPrefs.GetInt("Blue", 0) == 0 ||
                          PlayerPrefs.GetInt("Yellow", 0) == 0);
         if(!doNotInteract) SoundManager.Instance.ChangeMusic(endMusic);
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
         SoundManager.Instance.PlaySound(earthSound);
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
            doNotInteract = true;
            transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            GameManager.Instance.GoToFinal();
            playerTransform.GetComponent<PlayerController>().enabled = false;
            playerTransform.GetComponent<PlayerAnimationManager>().enabled = false;
            gemBag.gameObject.SetActive(false);
            SoundManager.Instance.PlaySound(explosionSound);
            Invoke(nameof(returnToLobby), 1.5f);
         }
      }
      public void EndInteraction()
      {
         Noise(0,0);
         SoundManager.Instance.StopEffect();
      }
   
      private void returnToLobby()
      {
         GameManager.Instance.LoadScene("LobbyColor");
      }
      
      private void Noise(float amplitudeGain, float frequencyGain) {
         noise.m_AmplitudeGain = amplitudeGain;
         noise.m_FrequencyGain = frequencyGain;    
      }
}
