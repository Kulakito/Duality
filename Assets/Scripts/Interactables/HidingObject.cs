using UnityEngine;

public class HidingObject : Interactable
{
    [field: SerializeField] public Transform HideSpot { get; private set;}
    [SerializeField] private Material _basicMat, _hideMat;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Interact()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();

        _meshRenderer.material = player.IsHiding ? _hideMat : _basicMat;
    }
}
