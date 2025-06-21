using UnityEngine;

public class TriggerObject : Interactable
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        _animator.SetTrigger("Open");
    }
}
