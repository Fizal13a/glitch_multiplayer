using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
   public static GameEvents gameEvents;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }

      if (gameEvents == null)
      {
         gameEvents = new GameEvents();
      }
   }
}
