using System;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private float throwForce = 10f;

    private Transform heldItem;

    public bool IsHoldingItem => heldItem != null;
    public IPickable HeldItem =>
        heldItem?.GetComponent<IPickable>();

    private void Start()
    {
        GameManager.gameEvents.AddEvent<Transform>(GameEvents.EventType.OnPickUp, PickUpItem);
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
        GameManager.gameEvents.TriggerEvent(GameEvents.EventType.OnDrop);

    }
    
    public IPickable ReleaseHeldItem()
    {
        if (heldItem == null)
            return null;

        IPickable item = heldItem.GetComponent<IPickable>();

        heldItem = null;

        return item;
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
        GameManager.gameEvents.TriggerEvent(GameEvents.EventType.OnDrop);
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