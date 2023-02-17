using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip gemSound;

    private void Awake()
    {
        PlayerController.OnCollectGem+=PlayGemSound;
    }

    private void PlayGemSound()
    {
        SoundManager.Instance.PlaySound(gemSound);
    }
}
