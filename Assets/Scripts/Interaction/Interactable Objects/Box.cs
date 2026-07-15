using System;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable, IPickable
{
    [Header("Components")]
    private MeshRenderer meshRenderer;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    
    public bool isPicked { get; set; }
    public bool CanInteract { get; }
    public float HoldDuration { get; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }


    public void Interact()
    {
        Debug.Log("Item Interacted");
        PickUp();
    }

    public void Highlight(bool enable)
    {
        meshRenderer.material = enable ? highlightMaterial : defaultMaterial;
    }

    public void PickUp()
    {
        GameManager.gameEvents.TriggerEvent<Transform>(GameEvents.EventType.OnPickUp, this.transform);
    }

    public void Drop()
    {
        GameManager.gameEvents.TriggerEvent<Transform>(GameEvents.EventType.OnDrop, this.transform);
    }
}
