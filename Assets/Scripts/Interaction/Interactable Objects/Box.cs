using System;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [Header("Components")]
    private MeshRenderer meshRenderer;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }

    public void Interact()
    {
        Debug.Log("Item Interacted");
        GameManager.gameEvents.TriggerEvent<Transform>(GameEvents.EventType.PickUp, this.transform);
    }

    public void Highlight(bool enable)
    {
        meshRenderer.material = enable ? highlightMaterial : defaultMaterial;
    }
}
