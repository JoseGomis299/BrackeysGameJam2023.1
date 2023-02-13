using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TrailFollower))]

public class PointEditor : Editor
{
   private void OnSceneGUI()
   {
      TrailFollower trailFollower = (TrailFollower)target;
      Handles.color = Color.green;

      var points = trailFollower.trailPoints;
      var currentPoint = trailFollower.currentPoint;
      
      for (int i = currentPoint; i < points.Length-1; i++)
      {
         Handles.DrawLine(points[i].transform.position, points[i+1].transform.position);
      }
      
      Handles.color = Color.blue;
      Handles.DrawLine(trailFollower.transform.position, points[currentPoint].transform.position);

   }
}
