using System.Collections;
using System.Collections.Generic;
using ProjectUtils.TopDown2D;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    private PlayerController _playerController;
    private Animator _animator;

    [SerializeField] private AudioClip walkSound;
    [SerializeField] private float walkSoundCoolDown;
    private float _lastWalkTime;

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
            }
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
    }
}
