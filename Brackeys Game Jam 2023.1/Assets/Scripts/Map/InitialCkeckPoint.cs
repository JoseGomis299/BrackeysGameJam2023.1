using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCkeckPoint : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SetCurrentCheckPoint(transform);
    }
}
