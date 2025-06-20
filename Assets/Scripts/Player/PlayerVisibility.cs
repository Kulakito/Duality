using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisibility : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction flipAction;

    public bool isGhost { get; private set; }
    [SerializeField] Material visibleMat, invisibleMat;
    [SerializeField] MeshRenderer rend;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        flipAction = playerInput.actions.FindAction("Flip");
    }

    void Update()
    {
        FlipPlayer();
    }

    void FlipPlayer()
    {
        if (playerInput.actions.actionMaps[0].name != "Player")
            return;
        bool isPressed = flipAction.WasPressedThisFrame();
        if (!isPressed)
            return;
        isGhost = !isGhost;
        if (isGhost)
            rend.material = invisibleMat;
        else
            rend.material = visibleMat;
    }
}
