using System;
using UnityEngine;

public class Fuse : PickableItem
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

    public override void Highlight(bool enable)
    {
        meshRenderer.material = enable
            ? highlightMaterial
            : defaultMaterial;
    }
}