using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    [SerializeField] float speed = 5f;
    [SerializeField] Transform meshTransform;

    Vector3 GetDirectionVector()
    {
        return new Vector3(moveAction.ReadValue<Vector2>().x, 0f, moveAction.ReadValue<Vector2>().y).normalized;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        if (playerInput.actions.actionMaps[0].name != "Player")
            return;
        Vector3 dir = GetDirectionVector();
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void RotatePlayer()
    {
        if (playerInput.actions.actionMaps[0].name != "Player")
            return;
        Vector3 dir = -GetDirectionVector();
        if (dir.magnitude == 0f)
            return;
        meshTransform.rotation = Quaternion.Slerp(meshTransform.rotation, Quaternion.LookRotation(dir), .15f);
    }
}
