using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    private IInteractable currentInteractable;
    
    public IInteractable CurrentInteractable => currentInteractable;

    private bool canFindObjects = false;

    private void Start()
    {
        canFindObjects = true;
        
        GameManager.gameEvents.AddEvent<Transform>(GameEvents.EventType.PickUp, OnPickUpItem);
        GameManager.gameEvents.AddEvent(GameEvents.EventType.Drop, OnDropItem);
    }

    private void Update()
    {
        FindClosestInteractable();
    }

    private void FindClosestInteractable()
    {
        if(!canFindObjects) return;
        
        IInteractable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (IInteractable interactable in interactables)
        {
            if (interactable == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                ((MonoBehaviour)interactable).transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }

        if (closest == currentInteractable)
            return;

        // Remove highlight from previous
        if (currentInteractable != null)
            currentInteractable.Highlight(false);

        currentInteractable = closest;

        // Highlight new
        if (currentInteractable != null)
            currentInteractable.Highlight(true);
    }

    public void Interact()
    {
        currentInteractable?.Interact();
    }

    private void OnPickUpItem(Transform interactable)
    {
        canFindObjects = false;
        RemoveAllInteractables();
    }
    
    private void OnDropItem()
    {
        canFindObjects = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!canFindObjects) return;

        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactables.Add(interactable);
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
    
    private void RemoveAllInteractables()
    {
        foreach (var interactable in interactables)
        {
            interactable?.Highlight(false);
        }

        interactables.Clear();
    }
}