using UnityEngine;

public interface IItemReceiver
{
    bool CanReceive(IPickable item);
    void Receive(IPickable item);
}
