using UnityEngine;

public class FuseSocket : TaskObjective, IInteractable, IItemReceiver
{
    [Header("Fuse Socket")]
    [SerializeField] private Transform fuseInsertPoint;

    [Header("Highlight")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material highlightMaterial;

    private Material defaultMaterial;

    public bool CanInteract => !IsCompleted;
    
    public float HoldDuration => 0f;

    private void Awake()
    {
        defaultMaterial = meshRenderer.material;
    }

    public bool CanReceive(IPickable item)
    {
        return item is Fuse && !IsCompleted;
    }

    public void Receive(IPickable item)
    {
        Fuse fuse = item as Fuse;

        if (fuse == null)
            return;

        InsertFuse(fuse);
    }

    private void InsertFuse(Fuse fuse)
    {
        Transform fuseTransform = fuse.transform;

        fuseTransform.SetParent(fuseInsertPoint);
        fuse.DisableInteraction();

        fuseTransform.localPosition = Vector3.zero;
        fuseTransform.localRotation = Quaternion.identity;

        Rigidbody rb = fuse.GetComponent<Rigidbody>();
        Collider col = fuse.GetComponent<Collider>();

        rb.isKinematic = true;
        rb.useGravity = false;

        col.enabled = false;

        Highlight(false);

        CompleteObjective();
    }

    public void Interact()
    {
        
    }

    public void Highlight(bool enable)
    {
        meshRenderer.material = enable
            ? highlightMaterial
            : defaultMaterial;
    }
}