public interface IInteractable
{
    float HoldDuration { get; }

    void Interact();
    void Highlight(bool enable);
}