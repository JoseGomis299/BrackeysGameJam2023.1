using System;
using FunkyCode;
using ProjectUtils.Attacking;
using ProjectUtils.TopDown2D;
using UnityEngine;
    public class PlayerController : Mover
    {
        private Vector3 _direction;
        private Vector3 _lastValidDirection;

        [SerializeField] private float dashCoolDown;
        private float _lastDashTime;
        private Vector3 _dashDirection;

        [SerializeField] private GameObject gemBag;
        public static event Action OnCollectGem;


        private void Awake()
        {
            EnemyFieldOfView.OnCanSeePlayer += ReceiveDamage;
            EnemyFieldOfView.OnCanBarelySeePlayer += ReceiveDamage;
            _lastValidDirection = Vector3.right;
        }

        void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            _direction = new Vector3(x, y, 0);
            if (_direction != Vector3.zero) _lastValidDirection = _direction;
            if (_direction.magnitude is <= 1 and > 0) _dashDirection = _direction;


            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - _lastDashTime >= dashCoolDown)
            {
                _lastDashTime = Time.time;
                Dash(_dashDirection);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                var overlap = Physics2D.OverlapCircleAll(transform.position, 3);
                foreach (var collider in overlap)
                {
                    var interactable = collider.GetComponent<Iinteractable>();
                    if(interactable != null) interactable.Interact();
                }
            }

        }
        private void FixedUpdate()
        {
            UpdateMotor(_direction);
        }

        protected override void Death()
        {
            if (!gemBag.activeInHierarchy)
            {
                gemBag.GetComponent<GemBag>().SetGems(GemManager.Instance.gemCount);
                GemManager.Instance.gemCount = 0;
                gemBag.transform.position = transform.position;
                capsuleCollider.enabled = false;
                gemBag.SetActive(true);
                OnCollectGem?.Invoke();
            }

            GameManager.Instance.Teleport(transform);
            Invoke(nameof(RestoreHealth), 0.7f);
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                ReceiveDamage(new Damage(transform.position, maxHealth, 0));
            }
            else if (other.CompareTag("Gem"))
            {
                GemManager.Instance.gemCount++;
                OnCollectGem?.Invoke();
                other.gameObject.SetActive(false);
            }
        }
    }

