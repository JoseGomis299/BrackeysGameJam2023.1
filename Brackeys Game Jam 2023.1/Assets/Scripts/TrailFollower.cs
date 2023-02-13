using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TrailFollower : MonoBehaviour
{
  public TrailPoint[] trailPoints;
  [SerializeField] private float speed;
  public int currentPoint { get; private set; }
  private Vector3 direction;

  private int targetAngle;

  private void Start()
  {
    currentPoint = 0;
    trailPoints[currentPoint].gameObject.SetActive(true);
    direction = (trailPoints[currentPoint].transform.position - transform.position).normalized;
    targetAngle = (int) (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    if (targetAngle < 0)
    {
      targetAngle += 360;
    }
  }

  private void FixedUpdate()
  {
    Vector3 targetPos = trailPoints[currentPoint].transform.position;
    float distance = Vector3.Distance(targetPos, transform.position);

    float temp = Mathf.Lerp(transform.localRotation.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 5);
    transform.localRotation = Quaternion.Euler(0f, 0f, temp);
    
    if (distance >= 0.05f)
    {
      Vector3 pos = transform.position + direction * (speed * Time.fixedDeltaTime);
      transform.position = pos;
    }
    else if(!trailPoints[currentPoint].startCount)
    {
      trailPoints[currentPoint].startCount = true;
      trailPoints[currentPoint].startTime = Time.time;
    }

    if (!trailPoints[currentPoint].canNext || distance >= 0.05f) return;

    trailPoints[currentPoint].canNext = false;
    trailPoints[currentPoint].startCount = false;
    currentPoint = ++currentPoint % trailPoints.Length;
    targetPos = trailPoints[currentPoint].transform.position;
    direction = (targetPos - transform.position).normalized;
    targetAngle = (int) (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    if (targetAngle < 0)
    {
      targetAngle += 360;
    }
  }


  public float getSpeed()
  {
    return speed;
  }
}

