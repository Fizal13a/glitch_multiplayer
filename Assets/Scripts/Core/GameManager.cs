using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
   
   public static readonly GameEvents gameEvents = new GameEvents();

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
   }

   private void Start()
   {
      gameEvents.TriggerEvent(GameEvents.EventType.OnGameStart);
   }
}
