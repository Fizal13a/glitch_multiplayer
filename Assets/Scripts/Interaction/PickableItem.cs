using UnityEngine;

public abstract class PickableItem : MonoBehaviour, IInteractable, IPickable
{
    public bool isPicked { get; set; }

    public bool CanInteract { get; protected set; } = true;
    public virtual float HoldDuration => 0f;

    public virtual void Interact()
    {
        if (!CanInteract)
            return;
        
        PickUp();
    }

    public virtual void PickUp()
    {
        GameManager.gameEvents.TriggerEvent<Transform>(
            GameEvents.EventType.OnPickUp,
            transform
        );
    }

    public virtual void Drop()
    {
        GameManager.gameEvents.TriggerEvent<Transform>(
            GameEvents.EventType.OnDrop,
            transform
        );
    }
    
    public void DisableInteraction()
    {
        CanInteract = false;
        Highlight(false);
    }

    public abstract void Highlight(bool enable);
}