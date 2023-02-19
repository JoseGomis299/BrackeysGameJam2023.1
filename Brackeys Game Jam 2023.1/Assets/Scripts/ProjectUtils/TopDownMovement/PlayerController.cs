using System;
using FunkyCode;
using ProjectUtils.Attacking;
using ProjectUtils.ObjectPooling;
using ProjectUtils.TopDown2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Mover
    {
        public Vector3 direction { get; private set; }
        private Vector3 _lastValidDirection;

        [SerializeField] private float dashCoolDown;
        private float _lastDashTime;
        private Vector3 _dashDirection;

        [SerializeField] private GameObject gemBag;

        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip dashSound;
        
        public bool inWheat { get; private set; }
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


            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - _lastDashTime >= dashCoolDown && _canDash)
            {
                _lastDashTime = Time.time;
                Dash(SceneManager.GetActiveScene().name == "Blue" ? _dashDirection : _lastValidDirection);
                SoundManager.Instance.PlaySound(dashSound);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(GameManager.Instance.paused) GameManager.Instance.ResumeGame();
                else GameManager.Instance.PauseGame();
            }
            
            var overlap = Physics2D.OverlapCircleAll(transform.position, 10);
            foreach (var collider in overlap)
            {
                var interactable = collider.GetComponent<Iinteractable>();
                if (interactable != null && capsuleCollider.enabled && interactable.DisplayButton())
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
                gemBag.SetActive(true);
                GemsCountUI.Instance.OnCollectGem();
            }

            capsuleCollider.enabled = false;
            SoundManager.Instance.PlaySound(deathSound);
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
            else if (other.CompareTag("Wheat"))
            {
                inWheat = true;
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        { 
            if (other.CompareTag("Wheat"))
            {
                inWheat = false;
            }       
        }

        public void OnCollectGemBag()
        {
            GemsCountUI.Instance.OnCollectGem();

            OnCollectGem?.Invoke();
        }
    }

