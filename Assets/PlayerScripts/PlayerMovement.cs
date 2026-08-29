using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //inputs and controls
    public InputActionAsset inputActions;
    private InputAction MoveAction;
    private InputAction LookAction;

    //movement
    public float basesensitivityX = 0.2f;
    public float basesensitivityY = 0.2f;
    [SerializeField] float movespeed = 4;

    public Transform Cam;
    public Transform trans;
    Rigidbody RB;
    Vector3 moveDirect;

    public bool ispaused;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        LookAction = InputSystem.actions.FindAction("Look");

        RB = GetComponent<Rigidbody>();
        Time.timeScale = 1f;
        ispaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Look()
    {
        Vector2 mouse = LookAction.ReadValue<Vector2>();     
        transform.Rotate(0, mouse.x * basesensitivityX*PlayerPrefs.GetFloat("CameraSensitivity", 1), 0);

        Vector3 rot = Cam.rotation.eulerAngles + new Vector3(mouse.y * basesensitivityY * -1 * PlayerPrefs.GetFloat("CameraSensitivity", 1), 0, 0);

        //Cam restrictions of going too high or too low
        if (rot.x < 280 && rot.x > 180)
        { //Debug.Log("TOO HIGH");//85 degrees above horizontal
            rot.x = 280;
        }
        else if (rot.x > 80 && rot.x < 180)
        {//Debug.Log("TOO LOW");//85 degrees below horizontal
            rot.x = 80;
        }
        if (rot.z > 1)//upside down for whatever reason
        {
            rot.z = 0;
            //Debug.LogWarning("CAM INVERTED AND CONTROLS ARE NOW INVERTED");
        }

        Cam.rotation = Quaternion.Euler(rot);
    }
    public void Movement()
    {
        Vector2 movevector = MoveAction.ReadValue<Vector2>();
        moveDirect = new Vector3(movevector.x, 0, movevector.y);
    }

    private void Update()
    {
        if (!ispaused)
        {
            Look();
        }
        
    }

    private void FixedUpdate()
    {        
        Movement();
        RB.MovePosition(transform.position + transform.TransformDirection(moveDirect) * (movespeed * Time.fixedDeltaTime));
    }
}