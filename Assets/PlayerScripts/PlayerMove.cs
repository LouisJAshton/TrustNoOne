using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
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
        if (disableMovement == 0 && MoveAction.WasPerformedThisFrame())
        {
            Debug.Log("MOVED");
            disableMovement = 22;
            NewPos = transform.position + transform.forward*4.5f;
            move = true;
        }
        else if (disableMovement == 0 && RotateLAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:L");
            disableMovement = 33;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y - 90;
            rotate = true;
        }
        else if (disableMovement == 0 && RotateRAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:R");
            disableMovement = 33;
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
        if (disableMovement>0)
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
}
