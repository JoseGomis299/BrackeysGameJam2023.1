using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    [SerializeField] private float movementMagnitude;
    public Vector3 initialPos;
    public void Move()
    {
        transform.position = (new Vector3(initialPos.x, initialPos.y+ Mathf.Sin(Time.time) * movementMagnitude, initialPos.z));
    }
}
