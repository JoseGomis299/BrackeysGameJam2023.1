using System.Collections;
using System.Collections.Generic;
using ProjectUtils.TopDown2D;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    private PlayerController _playerController;
    private Animator _animator;

    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip wheatSound;
    [SerializeField] private AudioClip iceSound;
    [SerializeField] private float walkSoundCoolDown;
    private float _lastWalkTime;
    [SerializeField] private float iceSoundCoolDown;
    private float _lastIceTime;


    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if(_playerController.GetHealth() <= 0)return;

        if (_playerController.state == Mover.MovementState.walking && _playerController.direction != Vector3.zero)
        {
            _animator.SetBool("Walking", true);
            if (Time.time - _lastWalkTime > walkSoundCoolDown)
            {
                _lastWalkTime = Time.time;
                SoundManager.Instance.PlaySound(walkSound);
                if(_playerController.inWheat) SoundManager.Instance.PlaySound(wheatSound);
            }
        }
        else
        {
            _animator.SetBool("Walking", false);
        }

        if (_playerController.state == Mover.MovementState.onIce && _playerController.moving)
        {
            if (Time.time - _lastIceTime > iceSoundCoolDown)
            {
                _lastIceTime = Time.time;
                SoundManager.Instance.PlaySound(iceSound);
            }

        }
    }
}
