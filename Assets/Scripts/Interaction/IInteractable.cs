public interface IInteractable
{
    bool CanInteract { get; }
    float HoldDuration { get; }

    void Interact();
    void Highlight(bool enable);
}