using System;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [Header("Pickup")] 
    [SerializeField] private Transform pickUpPoint;

    private void OnEnable()
    {
        GameManager.gameEvents.AddEvent<Transform>(GameEvents.EventType.PickUp, PickUpItem);
    }

    public void PickUpItem(Transform item)
    {
        Debug.Log("Picked up item");
        item.SetParent(pickUpPoint);
        item.localPosition = Vector3.zero;
    }
}
