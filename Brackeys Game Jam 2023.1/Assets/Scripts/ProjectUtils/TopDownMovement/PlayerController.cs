using System;
using FunkyCode;
using ProjectUtils.Attacking;
using ProjectUtils.ObjectPooling;
using ProjectUtils.TopDown2D;
using UnityEngine;
    public class PlayerController : Mover
    {
        public Vector3 direction { get; private set; }
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
            direction = new Vector3(x, y, 0);
            if (direction != Vector3.zero) _lastValidDirection = direction;
            if (direction.magnitude is <= 1 and > 0) _dashDirection = direction;


            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - _lastDashTime >= dashCoolDown)
            {
                _lastDashTime = Time.time;
                Dash(_dashDirection);
            }
            
            var overlap = Physics2D.OverlapCircleAll(transform.position, 3);
            foreach (var collider in overlap)
            {
                var interactable = collider.GetComponent<Iinteractable>();
                if (interactable != null && interactable.DisplayButton())
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.StartInteraction();
                    }

                    if (Input.GetKey(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                    
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        interactable.EndInteraction();
                    }
                }
            }
        }
        private void FixedUpdate()
        {
            UpdateMotor(direction);
        }

        protected override void Death()
        {
            if (!gemBag.activeInHierarchy && GemManager.Instance.gemCount > 0)
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
                ObjectPool.Instance.InstantiateFromPoolIndex(1, other.transform.position, Quaternion.identity, true).GetComponent<ParticleSystem>().Play();
                other.gameObject.SetActive(false);
            }
        }
    }

