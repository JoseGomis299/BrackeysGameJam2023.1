using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotRotateWithParent : MonoBehaviour
{
    private float offset;
    private Transform child;

    private void Awake()
    {
        child = transform.GetChild(1);
        offset = child.transform.localRotation.eulerAngles.z;
    }

    void LateUpdate()
    {
        child.transform.localRotation = Quaternion.Euler(0,0,offset-transform.localRotation.eulerAngles.z);
    }
}
