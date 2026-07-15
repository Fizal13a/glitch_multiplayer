using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    private PickupSystem pickupSystem;
    private IInteractable currentInteractable;

    public IInteractable CurrentInteractable => currentInteractable;

    private void Awake()
    {
        pickupSystem = GetComponent<PickupSystem>();
    }

    private void Update()
    {
        FindClosestInteractable();
    }

    private void FindClosestInteractable()
    {
        IInteractable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (IInteractable interactable in interactables)
        {
            if (interactable == null)
                continue;

            if (pickupSystem.IsHoldingItem &&
                interactable is not IItemReceiver)
            {
                continue;
            }

            float distance = Vector3.Distance(
                transform.position,
                ((MonoBehaviour)interactable).transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }

        if (closest == currentInteractable)
            return;

        if (currentInteractable != null)
        {
            currentInteractable.Highlight(false);
        }

        currentInteractable = closest;

        if (currentInteractable != null)
        {
            currentInteractable.Highlight(true);
        }
    }

    public void Interact()
    {
        currentInteractable?.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (!interactables.Contains(interactable))
            {
                interactables.Add(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactables.Remove(interactable);

            if (interactable == currentInteractable)
            {
                currentInteractable.Highlight(false);
                currentInteractable = null;
            }
        }
    }
}