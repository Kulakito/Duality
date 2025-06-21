using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] protected bool effectGhost, isActive;

    protected abstract void Perform();
    public abstract void ResetTrap();
}
