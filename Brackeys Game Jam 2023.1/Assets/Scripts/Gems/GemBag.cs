using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class GemBag : MonoBehaviour
{
   private Transform target;
   [SerializeField] private float speed;
   [SerializeField] private float movementMagnitude;
   private Vector3 initialPos;
   private float _speed;
   private int gemsNumber;

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
      initialPos = transform.position;
   }

   private void FixedUpdate()
   {
      if (target != null)
      {
         _speed = Mathf.Lerp(_speed, speed, Time.fixedDeltaTime * 5f);
         var direction = (target.position - transform.position).normalized;
         transform.Translate(direction * (_speed * Time.fixedDeltaTime));
         
         if (Vector3.Distance(transform.position, target.position) <= 1)
         {
            GemManager.Instance.gemCount += gemsNumber;
            gemsNumber = 0;
            target = null;
            GemsCountUI.Instance.OnCollectGem();
            ObjectPool.Instance.InstantiateFromPoolIndex(1, transform.position, Quaternion.identity, true).GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
         }
      }
      else
      {
         transform.position = (new Vector3(initialPos.x, initialPos.y+ Mathf.Sin(Time.time) * movementMagnitude, initialPos.z));
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
