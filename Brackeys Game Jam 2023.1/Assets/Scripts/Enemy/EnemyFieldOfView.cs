using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Attacking;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    public float extraRadius;
    [Range(0, 360)] public float angle;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;
    public static event Action<Damage>  OnCanSeePlayer;
    public static event Action<Damage> OnCanBarelySeePlayer;

    private Damage _extraDamage;
    private Damage _damage;


    private void Start()
    {
        _extraDamage = new Damage(transform.position, 1f, 0);
        _damage = new Damage(transform.position, 2f, 0);
    }

    private void Update()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        Collider2D[] ExtraRangeChecks = Physics2D.OverlapCircleAll(transform.position, extraRadius, targetLayer);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer))
                {
                    OnCanSeePlayer?.Invoke(_damage);
                }
            }
        }
        else if (ExtraRangeChecks.Length != 0)
        {
            Transform target = ExtraRangeChecks[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer))
                {
                    OnCanBarelySeePlayer?.Invoke(_extraDamage);
                }
            }
        }
    }

}
