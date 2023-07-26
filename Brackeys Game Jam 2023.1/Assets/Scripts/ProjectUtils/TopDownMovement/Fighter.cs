using System;
using System.Collections;
using FunkyCode;
using FunkyCode.Rendering.Day;
using ProjectUtils.Attacking;
using UnityEngine;

namespace ProjectUtils.TopDown2D
{
    public class Fighter : MonoBehaviour, IDamageable
    {
        [Header("Fighting")]
        [SerializeField] protected float health = 10;
        [SerializeField] protected float maxHealth = 10;

        [SerializeField] protected float immuneTime = 0.05f;
        protected float lastImmune;

        protected Vector3 pushDirection;
        protected GameObject fighterGFX;
        protected SpriteRenderer fighterGFXRenderer;
        protected Color fighterColor;

        [SerializeField] private float healthRecovery = 1;
        [SerializeField] private float _recoveringStartTime = 5;
        [SerializeField] private float recoveryTime = 1;
        
        protected CapsuleCollider2D capsuleCollider;
        private Light2D _playerLight;
        private float _playerLightAlpha;

        protected virtual void Start()
        {
            fighterGFX = transform.GetChild(0).gameObject;
            fighterGFXRenderer = fighterGFX.GetComponent<SpriteRenderer>();
            fighterColor = fighterGFXRenderer.color;

            lastImmune = float.MinValue;

            if (transform.CompareTag("Player"))
            {
                _playerLight = transform.GetChild(1).GetComponent<Light2D>();
                _playerLightAlpha = _playerLight.color.a;
            }

            InvokeRepeating(nameof(HealthRecovery), recoveryTime, recoveryTime);
        }

        
        public virtual void ReceiveDamage(Damage dmg)
        {
            if (this != null && Time.time - lastImmune > immuneTime)
            {
                health -= dmg.damageAmount;
                fighterGFXRenderer.color = new Color(fighterColor.r, fighterColor.g, fighterColor.b, health / maxHealth);
                if (transform.CompareTag("Player") && _playerLight.color.a > 0.15f) _playerLight.color.a = _playerLightAlpha * (health / maxHealth);

                if (health <= 0)
                {
                    health = 0;
                    Death();
                }

            }
        }

        protected void RestoreHealth()
        {
            health = maxHealth;
            fighterGFXRenderer.color = new Color(fighterColor.r, fighterColor.g, fighterColor.b, 1);
            if (transform.CompareTag("Player")) _playerLight.color.a = _playerLightAlpha;
            capsuleCollider.enabled = true;
        }

        private void HealthRecovery()
        {
            if (health < maxHealth && Time.time - lastImmune >= _recoveringStartTime && health > 0)
            {
                if (health + healthRecovery > maxHealth) health = maxHealth;
                else health += healthRecovery;
                fighterGFXRenderer.color = new Color(fighterColor.r, fighterColor.g, fighterColor.b, health / maxHealth);
                if(transform.CompareTag("Player")) _playerLight.color.a = _playerLightAlpha * (health / maxHealth);

            }
        }

        protected IEnumerator ImmuneDisplay()
        {
            float elapsedTime = 0;
            WaitForSeconds waitForSeconds = new WaitForSeconds(immuneTime / 10f);
            
            while (elapsedTime < immuneTime)
            {
                fighterGFXRenderer.color = new Color(1, 1, 1, 0);
                elapsedTime += immuneTime / 5f;
                yield return waitForSeconds;
                fighterGFXRenderer.color = new Color(1, 1, 1, health / maxHealth);
                yield return waitForSeconds;
            }

            fighterGFXRenderer.color = new Color(1, 1, 1, health / maxHealth);
        }

        public float GetHealth()
        {
            return health;
        }
        protected virtual void Death()
        {
      
        }
    }
}