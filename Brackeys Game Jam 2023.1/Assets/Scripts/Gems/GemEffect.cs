using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemEffect : MonoBehaviour
{
   private ParticleSystem _particleSystem;

  private Animator _blue;
  private Animator _red;
  private Animator _yellow;
  private static readonly int ChangeColor = Animator.StringToHash("ChangeColor");

  private void Start()
   {
      _particleSystem = GetComponentInChildren<ParticleSystem>();
      Altar.OnAltarGot += OnGemEffect;

      var color = GameObject.FindWithTag("Colors");
      _red = color.transform.GetChild(0).GetComponent<Animator>();
      _yellow = color.transform.GetChild(1).GetComponent<Animator>();
      _blue = color.transform.GetChild(2).GetComponent<Animator>();
   }


  private void OnGemEffect(char color)
   {
      switch (color)
      {
         case 'b':  _particleSystem.startColor = Color.blue;
                        _blue.SetBool(ChangeColor, true);
                        PlayerPrefs.SetInt("Blue", 1);
            break;
         case 'r':  _particleSystem.startColor = Color.red;
            _red.SetBool(ChangeColor, true);
            PlayerPrefs.SetInt("Red", 1);
            break;
         case 'y':  _particleSystem.startColor = Color.yellow;
            _yellow.SetBool(ChangeColor, true); 
            PlayerPrefs.SetInt("Yellow", 1);
            break;
      }
      _particleSystem.Play();
   }
}
