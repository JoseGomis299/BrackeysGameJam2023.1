using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBag : MonoBehaviour
{
   private Transform target;
   [SerializeField] private float speed;
   private float _speed;
   private int gemsNumber;

   public void SetGems(int gemsNumber)
   {
      this.gemsNumber = gemsNumber;
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
            target = null;
            gameObject.SetActive(false);
         }
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
