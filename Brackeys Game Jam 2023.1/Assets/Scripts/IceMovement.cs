using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.TopDown2D;
using UnityEngine;

public class IceMovement : MonoBehaviour
{
    private Mover _mover;
    [SerializeField] private float iceSpeed;
    private Vector3 _iceDirection;
    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    public Vector3 Move(Vector3 input)
    {
        if (_mover.state == Mover.MovementState.onIce)
        {
            _mover.SetCanDash(false);
            if (_iceDirection.magnitude <= 0.1f)
            {
                if (input.x != 0 && input.y != 0) input.x = 0;
                _iceDirection = input * iceSpeed;
            }
            else
            {
                input = _iceDirection;
            }

            if (Physics2D.CapsuleCast(new Vector3(transform.position.x+_mover.GetCapsuleCollider2D().offset.x, transform.position.y+_mover.GetCapsuleCollider2D().offset.y, 0), _mover.GetCapsuleCollider2D().size*transform.localScale, _mover.GetCapsuleCollider2D().direction, 0,
                    _iceDirection.normalized, 0.1f, _mover.collisionLayer))
            {
                _iceDirection = Vector3.zero;
            }
        }
   
        return input; 
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.CompareTag("Ice"))
        {
            _mover.state = Mover.MovementState.onIce;
        }
    }
           
    private void OnTriggerExit2D(Collider2D other)
    {
        var list = new List<Collider2D>();
        _mover.GetCapsuleCollider2D().OverlapCollider(new ContactFilter2D().NoFilter(), list);
        foreach (var col in list)
        {
            Debug.Log(col.name);
            if (col.CompareTag("Ice"))
            {
                return;
            }
        } 
        if (other.CompareTag("Ice"))
        {
            _mover.state = Mover.MovementState.walking;
            _iceDirection = Vector3.zero;
            _mover.SetCanDash(_mover.canDash);
        }
    }
}
