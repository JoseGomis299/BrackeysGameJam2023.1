using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class GemBag : MonoBehaviour
{
   private Transform target;
   [SerializeField] private float speed;
   
   private float _speed;
   private int gemsNumber;

   private SineMovement _sineMovement;

   public bool DontGo;

   public void SetGems(int gemsNumber)
   {
      this.gemsNumber = gemsNumber;
   }
   
   public int GetGems()
   {
      return gemsNumber;
   }

   private void OnEnable()
   {
      _sineMovement = GetComponent<SineMovement>();
      _sineMovement.initialPos = transform.position;
   }

   private void FixedUpdate()
   {
      if (target != null && !DontGo)
      {
         _speed = Mathf.Lerp(_speed, speed, Time.fixedDeltaTime * 5f);
         var direction = (target.position - transform.position).normalized;
         transform.Translate(direction * (_speed * Time.fixedDeltaTime));
         
         if (Vector3.Distance(transform.position, target.position) <= 1)
         {
            GemManager.Instance.gemCount += gemsNumber;
            gemsNumber = 0;
            target = null;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().OnCollectGemBag();
            ObjectPool.Instance.InstantiateFromPoolIndex(1, transform.position, Quaternion.identity, true).GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
         }
      }
      else
      {
         _sineMovement.Move();
      }
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         target = col.transform;
      }
   }
}
