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
        [SerializeField] private float dashForce = 35;
        [SerializeField] private float dashDuration= 0.5f;
        private Vector3 _dashDirection;
        [SerializeField] private GameObject dashEcho;
        private Color moverColor;
        protected float lastDashTime;
        private Rigidbody2D _rb;



        private CapsuleCollider2D _capsuleCollider;

        [Header("States")]
        public MovementState state;
        public enum MovementState
        {
            walking,
            dashing
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
            if(!canDash) return;
            
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
    }
}