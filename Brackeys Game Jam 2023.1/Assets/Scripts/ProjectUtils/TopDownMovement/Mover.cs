using System;
using System.Collections.Generic;
using ProjectUtils.Attacking;
using ProjectUtils.ObjectPooling;
using UnityEngine;

namespace ProjectUtils.TopDown2D
{
    public abstract class Mover : Fighter
    {
        [Header("Movement")]
        [SerializeField] protected float ySpeed = 3.75f;
        [SerializeField] protected float xSpeed = 4;
        [SerializeField] public LayerMask collisionLayer;
        private Vector3 _moveDelta;
        private RaycastHit2D _hit;
        [SerializeField] public bool canDash = true;
        protected bool _canDash = true;
        [SerializeField] private float dashForce = 35;
        [SerializeField] private float dashDuration= 0.5f;
        private Vector3 _dashDirection;
        [SerializeField] private GameObject dashEcho;
        protected float lastDashTime;
        private Rigidbody2D _rb;
        private IceMovement _iceMovement;


        [Header("States")]
        public MovementState state;
        public enum MovementState
        {
            walking,
            dashing,
            onIce
        }
        protected virtual void Start()
        {
            base.Start();
            capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _iceMovement = GetComponent<IceMovement>();
        }

        protected void UpdateMotor(Vector3 input)
        {
            if(health <= 0) return;
            if (Time.time - lastDashTime <= dashDuration)
            {
                DoDash();
                return;
            } 
            if (state == MovementState.dashing)
            {
                state = MovementState.walking;
            }

            if(_iceMovement != null) input = _iceMovement.Move(input);
            Move(input);
        }
        private void Move(Vector3 input)
        {
            _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

            //Changing GFX direction
            if (_moveDelta.x < 0)
            {
                fighterGFX.transform.localScale = new Vector3(-1, 1, 1) ;
            }
            else if (_moveDelta.x > 0)
            {
                fighterGFX.transform.localScale = new Vector3(1, 1, 1);
            }
            // else if (_moveDelta.y > 0)
            // {
            //     //change sprite to walking upwards
            // }
            // else if (_moveDelta.y < 0)
            // {
            //     //change sprite to walking downards
            // }
            
            //Move position
            _rb.MovePosition(transform.position + _moveDelta * Time.fixedDeltaTime);
        }
        protected void Dash(Vector3 direction)
        {
            if(!canDash||!_canDash) return;
            
            _dashDirection = dashForce*direction;
            state = MovementState.dashing;
            lastDashTime = Time.time;
        }
        private void DoDash()
        {
            _rb.MovePosition(transform.position + _dashDirection*Time.fixedDeltaTime);

            if (dashEcho != null)
            {
                var echo = ObjectPool.Instance.InstantiateFromPool(dashEcho, transform.position, Quaternion.identity, true);
                echo.transform.rotation = fighterGFX.transform.rotation;
                echo.transform.localScale = new Vector3(fighterGFX.transform.localScale.x * transform.localScale.x, fighterGFX.transform.localScale.y * transform.localScale.y,fighterGFX.transform.localScale.z * transform.localScale.z);
                var echoRenderer = echo.GetComponent<SpriteRenderer>();
                echoRenderer.sprite = fighterGFX.GetComponent<SpriteRenderer>().sprite;
                echoRenderer.color = new Color(fighterColor.r, fighterColor.g, fighterColor.b, fighterColor.a*0.5f);
            }
        }

        public void SetCanDash(bool value)
        {
            _canDash = value;
        }

        public CapsuleCollider2D GetCapsuleCollider2D()
        {
            return capsuleCollider;
        }
    }
}