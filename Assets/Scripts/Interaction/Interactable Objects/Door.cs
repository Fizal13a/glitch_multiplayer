using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Components")]
    private MeshRenderer meshRenderer;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    public bool CanInteract { get; }
    public float HoldDuration { get; }
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }
    public void Interact()
    {
        Debug.Log("Door Interact");
    }

    public void Highlight(bool enable)
    {
        meshRenderer.material = enable ? highlightMaterial : defaultMaterial;
    }
}
