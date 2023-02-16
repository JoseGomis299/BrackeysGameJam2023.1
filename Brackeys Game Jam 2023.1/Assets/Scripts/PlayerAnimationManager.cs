using System.Collections;
using System.Collections.Generic;
using ProjectUtils.TopDown2D;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    private PlayerController _playerController;
    private Animator _animator;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        _animator.SetBool("Walking", _playerController.state == Mover.MovementState.walking && _playerController.direction != Vector3.zero);
    }
}
