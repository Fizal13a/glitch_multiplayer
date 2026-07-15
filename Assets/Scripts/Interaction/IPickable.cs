using UnityEngine;

public interface IPickable
{
    bool isPicked { get; set; }
    
    void PickUp();
    void Drop();
}
