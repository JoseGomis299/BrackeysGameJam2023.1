using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.TopDown2D;
using UnityEngine;
using UnityEngine.Rendering;

public class TrailFollower : Mover
{
  public TrailPoint[] trailPoints;
  public int currentPoint { get; private set; }
  private Vector3 direction;

  private float targetAngle;
  private void Start()
  {
    base.Start();
    
    if(trailPoints.Length <= 0) return;
    
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
    if(trailPoints.Length <= 0)return;
    Vector3 targetPos = trailPoints[currentPoint].transform.position;
    float distance = Vector3.Distance(targetPos, transform.position);

    float temp = Mathf.Lerp(transform.localRotation.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 10);
    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.x, temp);
    
    if (distance >= 0.05f)
    {
      UpdateMotor(direction);
    }
    else if(!trailPoints[currentPoint].startCount)
    {
      trailPoints[currentPoint].startCount = true;
      trailPoints[currentPoint].startTime = Time.time;
    }

    if (!trailPoints[currentPoint].canNext || distance >= 0.05f) return;

    trailPoints[currentPoint].canNext = false;
    trailPoints[currentPoint].startCount = false;
    trailPoints[currentPoint].gameObject.SetActive(false);

    currentPoint = ++currentPoint % trailPoints.Length;
    trailPoints[currentPoint].gameObject.SetActive(true);
    targetPos = trailPoints[currentPoint].transform.position;
    direction = (targetPos - transform.position).normalized;
    targetAngle = (int) (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    if (targetAngle < 0)
    {
      targetAngle += 360;
    }
  }
  
}

