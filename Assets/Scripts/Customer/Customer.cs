using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
   [SerializeField] private Vector2 waitRange = new Vector2(15, 20);
   private float waitTime = 15;
   private float currentWaitTime = 0;
   
   private Coroutine waitRoutine;
   private CustomerManager customerManager;

   public void InitializeCustomer(CustomerManager manager)
   {
      customerManager = manager;
   }

   public void StartWaiting()
   {
      waitTime = Random.Range(waitRange.x, waitRange.y);
      currentWaitTime = waitTime;

      if(waitRoutine != null)
      {
         StopCoroutine(waitRoutine);
         waitRoutine = null;
      }
      
      waitRoutine = StartCoroutine(WaitTimeLoop());
   }

   IEnumerator WaitTimeLoop()
   {
      while (currentWaitTime > 0)
      {
         currentWaitTime -= Time.deltaTime;
         float patience = currentWaitTime / waitTime;
         customerManager.customerUIManager.SetCustomerPatienceLevel(patience);
         yield return null;
      }
      
      waitRoutine = null;
      OnWaitTimeOver();
   }
   
   private void OnWaitTimeOver()
   {
      customerManager.MoveCustomers();
   }

   private void OnItemsDelivered()
   {
      if(waitRoutine != null)
      {
         StopCoroutine(waitRoutine);
         waitRoutine = null;
      }
   }
   
}
