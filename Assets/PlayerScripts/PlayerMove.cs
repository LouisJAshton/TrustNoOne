using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

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

    private void Update()
    {
        if (disableMovement == 0 && MoveAction.WasPerformedThisFrame())
        {
            Debug.Log("MOVED");
            disableMovement = 40;
            NewPos = gameObject.transform.forward*10;
            move = true;
        }
        else if (disableMovement == 0 && RotateLAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:L");
            disableMovement = 40;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y - 90;
            rotate = true;
        }
        else if (disableMovement == 0 && RotateRAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:R");
            disableMovement = 40;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y + 90;
            rotate = true;
        }

        if (move)
        {
            currentPos = transform.position;
            targetPos = NewPos;
            Vector3 mov = Vector3.Slerp(currentPos, targetPos, 0.08f);
            transform.position = mov;
        }

        if (rotate)
        {
            currentRot = transform.rotation;
            targetRot = Quaternion.Euler(NewRot);
            Quaternion Rot = Quaternion.Slerp(currentRot, targetRot, 0.08f);
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
            //transform.position = targetPos;
           // move = false;
            rotate = false;
        }
    }
}
