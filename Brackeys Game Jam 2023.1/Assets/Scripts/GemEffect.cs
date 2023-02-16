using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemEffect : MonoBehaviour
{
   private ParticleSystem _particleSystem;

  [SerializeField] private Animator _blue;
  [SerializeField] private Animator _red;
  [SerializeField] private Animator _yellow;
  private void Start()
   {
      _particleSystem = GetComponentInChildren<ParticleSystem>();
   }


  private void OnGemEffect(char color)
   {
      switch (color)
      {
         case 'b':  _particleSystem.startColor = Color.blue;
                        _blue.SetBool("ChangeColor", true);
                        PlayerPrefs.SetInt("Blue", 1);
            break;
         case 'r':  _particleSystem.startColor = Color.red;
            _red.SetBool("ChangeColor", true);
            PlayerPrefs.SetInt("Red", 1);
            break;
         case 'y':  _particleSystem.startColor = Color.yellow;
            _yellow.SetBool("ChangeColor", true); 
            PlayerPrefs.SetInt("Yellow", 1);
            break;
      }
      _particleSystem.Play();
   }
}
