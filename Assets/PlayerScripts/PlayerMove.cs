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
    private Quaternion targetRot = new Quaternion(0, -0.70711f, 0, 0.70711f);//starting rotation
    private Vector3 currentPos;
    private Vector3 targetPos = new Vector3 (0, 4, 0);//starting position

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
        transform.position = new Vector3(0, 4, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void Update()
    {
        if (disableMovement == 0 && MoveAction.WasPerformedThisFrame())
        {
            Debug.Log("MOVED");
            disableMovement = 20;
            NewPos = transform.position + transform.forward*4;
            move = true;
        }
        else if (disableMovement == 0 && RotateLAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:L");
            disableMovement = 25;
            NewRot = currentRot.eulerAngles;
            NewRot.y = NewRot.y - 90;
            rotate = true;
        }
        else if (disableMovement == 0 && RotateRAction.WasPerformedThisFrame())
        {
            Debug.Log("ROTATE:R");
            disableMovement = 25;
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
