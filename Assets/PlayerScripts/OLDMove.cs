using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private LayerMask mask;


    private InputAction MoveAction;
    private InputAction RotateLAction;
    private InputAction RotateRAction;

    private Quaternion currentRot;
    private Quaternion targetRot;
    private Vector3 currentPos;
    private Vector3 targetPos;

    private Vector3 NewRot;
    private Vector3 NewPos;

    private int disableMovement;
    private bool move = false;
    private bool rotate = false;
    private bool blocked = false;

    private void Awake()
    {
        inputActions.FindActionMap("Player").Enable();

        MoveAction = InputSystem.actions.FindAction("Forward");
        RotateLAction = InputSystem.actions.FindAction("Left");
        RotateRAction = InputSystem.actions.FindAction("Right");
    }

    private void Start()
    {
        targetPos = transform.position;
        targetRot = transform.rotation;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position - new Vector3(0, 0.45f, 0), transform.forward * 3, out RaycastHit hit, mask))
        {
            blocked = true;
        }
        else
        {
            blocked = false;
        }


        if (disableMovement == 0 && MoveAction.IsPressed() && !blocked)
        {
            Debug.Log("MOVED");
            disableMovement = 22;
            NewPos = transform.position + transform.forward * 4.5f;
            move = true;
        }
        else if (disableMovement == 0 && RotateLAction.IsPressed())
        {
            Debug.Log("ROTATE:L");
            disableMovement = 35;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y - 90;
            rotate = true;
        }
        else if (disableMovement == 0 && RotateRAction.IsPressed())
        {
            Debug.Log("ROTATE:R");
            disableMovement = 35;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y + 90;
            rotate = true;
        }

        if (move)
        {
            currentPos = transform.position;
            targetPos = NewPos;
            Vector3 mov = Vector3.Lerp(currentPos, targetPos, 0.1f);
            transform.position = mov;
        }

        if (rotate)
        {
            currentRot = transform.rotation;
            targetRot = Quaternion.Euler(NewRot);
            Quaternion Rot = Quaternion.Slerp(currentRot, targetRot, 0.1f);
            transform.rotation = Rot;
        }
    }

    private void FixedUpdate()
    {
        if (disableMovement > 0)
        {
            disableMovement--;
        }
        else
        {
            transform.rotation = targetRot;
            transform.position = targetPos;
            move = false;
            rotate = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - new Vector3(0, 0.45f, 0), transform.forward * 4f);
    }
}
