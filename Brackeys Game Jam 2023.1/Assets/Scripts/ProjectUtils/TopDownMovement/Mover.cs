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
        private Vector3 _moveDelta;
        [SerializeField] protected float ySpeed = 3.75f;
        [SerializeField] protected float xSpeed = 4;
        [SerializeField] private LayerMask collisionLayer;
        private RaycastHit2D _hit;
        [SerializeField] public bool canDash = true;
        [SerializeField] public bool _canDash = true;
        [SerializeField] private float dashForce = 35;
        [SerializeField] private float dashDuration= 0.5f;
        private Vector3 _dashDirection;
        [SerializeField] private GameObject dashEcho;
        private Color moverColor;
        protected float lastDashTime;
        private Rigidbody2D _rb;
        [SerializeField] private bool slidesOnIce;
        [SerializeField] private float iceSpeed;
        private Vector3 _iceDirection;


        private CapsuleCollider2D _capsuleCollider;

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
            _capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
            moverColor = fighterGFX.GetComponent<SpriteRenderer>().color;
            _rb = GetComponent<Rigidbody2D>();
        }

        protected void UpdateMotor(Vector3 input)
        {
            if (Time.time - lastDashTime <= dashDuration)
            {
                DoDash();
                return;
            } 
            if (state == MovementState.dashing)
            {
                state = MovementState.walking;
            }

            input = IceMovement(input);
            Move(input);
        }



        private void Move(Vector3 input)
        {
            _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

            //Changing GFX direction
            if (_moveDelta.x < 0)
            {
                fighterGFX.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_moveDelta.x > 0)
            {
                fighterGFX.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_moveDelta.y > 0)
            {
                fighterGFX.transform.localScale = new Vector3(1, 1, 1);
                //change sprite to walking upwards
            }
            else if (_moveDelta.y < 0)
            {
                fighterGFX.transform.localScale = new Vector3(1, 1, 1);
                //change sprite to walking downards
            }
            
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
                echo.transform.localScale = fighterGFX.transform.localScale;
                var echoRenderer = echo.GetComponent<SpriteRenderer>();
                echoRenderer.sprite = fighterGFX.GetComponent<SpriteRenderer>().sprite;
                echoRenderer.color = new Color(moverColor.r, moverColor.g, moverColor.b, 0.5f);
            }
        }

        private Vector3 IceMovement(Vector3 input)
        {
            if (slidesOnIce && state == MovementState.onIce)
            {
                _canDash = false;
                if (_iceDirection.magnitude <= 0.1f)
                {
                    if (input.x != 0 && input.y != 0) input.x = 0;
                    _iceDirection = input * iceSpeed;
                }
                else
                {
                    input = _iceDirection;
                }

                if (Physics2D.CapsuleCast(transform.position, _capsuleCollider.size, _capsuleCollider.direction, 0,
                        _iceDirection.normalized, 0.1f, collisionLayer))
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
                state = MovementState.onIce;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var list = new List<Collider2D>();
            _capsuleCollider.OverlapCollider(new ContactFilter2D().NoFilter(), list);
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
                state = MovementState.walking;
                _iceDirection = Vector3.zero;
                _canDash = canDash;
            }
        }
    }
}