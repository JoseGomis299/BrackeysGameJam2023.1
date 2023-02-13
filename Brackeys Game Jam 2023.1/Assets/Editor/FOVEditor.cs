using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyFieldOfView))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFieldOfView fov = (EnemyFieldOfView)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.radius);
        Handles.color = Color.magenta;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.extraRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.extraRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.extraRadius);

    }

    private Vector3 DirectionFromAngle(float eulerAnglesZ, float angleInDegrees)
    {
        angleInDegrees += eulerAnglesZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
