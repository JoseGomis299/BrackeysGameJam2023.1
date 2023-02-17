using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public Type volumeType;
    public enum Type
    {
        Master,
        Music,
        Effects
    }
    void Start()
    {
        switch (volumeType)
        {
            case Type.Master: _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
                break;
            case Type.Music: _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
                break;
            case Type.Effects: _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectsVolume(val));
                break;
        }
        
    }

}
