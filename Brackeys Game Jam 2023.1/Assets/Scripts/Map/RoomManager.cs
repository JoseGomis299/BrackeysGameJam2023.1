using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
   [SerializeField] private GameObject[] roomObjects;
   [SerializeField] private int[] objectsRange;

   private GameObject[,] _roomObjects;
   private void Start()
   {
      Door.OnChangeRoom += ChangeRoom;

      var index = 0;
      _roomObjects = new GameObject[roomObjects.Length, roomObjects.Length];
      for (int i = 0; i < objectsRange.Length; i++)
      {
         for (int j = 0; j < objectsRange[i]; j++)
         {
            _roomObjects[i,j] = roomObjects[index]; 
            index++;
            if (i != 0)
            {
               _roomObjects[i,j].SetActive(false);
            }
         }
      }
   }

   private void ChangeRoom(int room, int nextRoom)
   {
      for (int i = 0; i < objectsRange[room]; i++) 
      {
            _roomObjects[room, i].SetActive(false);
      }
      for (int i = 0; i < objectsRange[nextRoom]; i++) 
      {
         _roomObjects[nextRoom, i].SetActive(true);
      }
   }
}
