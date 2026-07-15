using System;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private float throwForce = 10f;

    private Transform heldItem;

    public bool IsHoldingItem => heldItem != null;

    private void Start()
    {
        GameManager.gameEvents.AddEvent<Transform>(GameEvents.EventType.PickUp, PickUpItem);
    }

    public void PickUpItem(Transform item)
    {
        heldItem = item;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        item.GetComponent<Collider>().enabled = false;

        item.SetParent(pickUpPoint);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }

    public void DropHeldItem()
    {
        if (heldItem == null)
            return;
        
        DropItem(heldItem);
        heldItem = null;
        GameManager.gameEvents.TriggerEvent(GameEvents.EventType.Drop);

    }

    public void ThrowHeldItem()
    {
        if (heldItem == null)
            return;

        Transform item = heldItem;
        heldItem = null;

        item.SetParent(null);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider col = item.GetComponent<Collider>();

        col.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        GameManager.gameEvents.TriggerEvent(GameEvents.EventType.Drop);
    }

    private void DropItem(Transform item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider col = item.GetComponent<Collider>();

        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;

        item.SetParent(null);
    }
}