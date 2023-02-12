using ProjectUtils.TopDown2D;
using UnityEngine;
    public class PlayerController : Mover
    {
        private Vector3 _direction;
        private Vector3 _lastValidDirection;

        [SerializeField] private float dashCoolDown;
        private float _lastDashTime;

        private void Awake()
        {
            _lastValidDirection = Vector3.right;
        }

        void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            _direction = new Vector3(x, y, 0);
            if (_direction != Vector3.zero) _lastValidDirection = _direction;


            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - _lastDashTime >= dashCoolDown)
            {
                _lastDashTime = Time.time;
                Dash(_lastValidDirection);
            }

            if (Input.GetMouseButtonDown(0))
            {
                
            }

            if (Input.GetMouseButtonDown(1))
            {
               
            }
        }

        private void FixedUpdate()
        {
            UpdateMotor(_direction);
        }
        
    }

