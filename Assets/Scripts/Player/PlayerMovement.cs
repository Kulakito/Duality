using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    [SerializeField] float speed = 5f;
    [SerializeField] Transform meshTransform;

    [SerializeField] float interactRange;

    public bool IsHiding { get; private set; }
    private HidingObject _hidingObject;
    private Vector3 _originalPos;

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
        if (IsHiding) return;

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

    private void OnInteract()
    {
        if (_hidingObject == null)
        {
            if (Physics.Raycast(meshTransform.position, -meshTransform.forward, out RaycastHit hit, interactRange) && hit.collider.TryGetComponent(out Interactable interactable))
            {
                if (interactable is HidingObject hidingObject)
                {
                    _hidingObject = hidingObject;

                    _originalPos = transform.position;
                    transform.position = hidingObject.HideSpot.position;

                    IsHiding = true;
                    hidingObject.Interact();
                }
                else interactable.Interact();
            }
        }
        else
        {
            transform.position = _originalPos;

            IsHiding = false;
            _hidingObject.Interact();
            _hidingObject = null;
            _originalPos = Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(meshTransform.position, -meshTransform.forward * interactRange);
    }

}
