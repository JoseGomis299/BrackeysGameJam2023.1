using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotRotateWithParent : MonoBehaviour
{
    private float[] offsets;
    [SerializeField] private Transform[] children;

    private void Awake()
    {
        offsets = new float[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            offsets[i] = children[i].transform.localRotation.eulerAngles.z;
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].transform.localRotation = Quaternion.Euler(0,0,offsets[i]-transform.localRotation.eulerAngles.z);
        }
    }
}
