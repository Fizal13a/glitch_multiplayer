using System;
using UnityEngine;
using DG.Tweening;

public class CustomerManager : MonoBehaviour
{
    public CustomerUIManager customerUIManager;
    
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private int maxCustomerCount = 5;
    [SerializeField] private Transform counterTransform;
    [SerializeField] private int customerOffset = 2;

    private Customer[] customers;

    private void OnEnable()
    {
        GameManager.gameEvents.AddEvent(GameEvents.EventType.OnGameStart, InitializeCustomers);
    }

    private void InitializeCustomers()
    {
        customers = new Customer[maxCustomerCount];
        
        for (int i = 0; i < maxCustomerCount; i++)
        {
            Vector3 spawnPosition = counterTransform.position;
            spawnPosition.z -= (i * customerOffset);
            GameObject newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
            Customer customer = newCustomer.GetComponent<Customer>();
            if (customer != null)
            {
                customer.InitializeCustomer(this);
                customers[i] = customer;
            }
        }

        CheckWaitingCustomer();
    }

    private void CheckWaitingCustomer()
    {
        Debug.Log("Checking customers");
        customerUIManager.ResetCustomerPatienceLevel();

        foreach (var customer in customers)
        {
            if (customer.transform.position.z > counterTransform.position.z)
            {
                Debug.Log("Customer Ahead");

                Vector3 newPosition = customer.transform.position;
                newPosition.z -= (customerOffset * customers.Length);
                customer.transform.position = newPosition;
            }

            if (customer.transform.position.z == counterTransform.position.z)
            {
                Debug.Log("Customer On Counter");

                customer.StartWaiting();
            }
        }
    }

    public void MoveCustomers()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var customer in customers)
        {
            Vector3 newPosition = customer.transform.position;
            newPosition.z += customerOffset;

            sequence.Join(
                customer.transform
                    .DOMove(newPosition, 2f)
                    .SetEase(Ease.OutExpo)
            );
        }

        sequence.OnComplete(() =>
        {
            Debug.Log("All customers moved.");

            CheckWaitingCustomer();
        });
    }
}
