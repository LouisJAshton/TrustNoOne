using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction InteractAction;

    private GameObject lastseen;
    private bool LookingAtSomeone;
    public bool istalking = false;

    private void Awake()
    {
        inputActions.FindActionMap("Player").Enable();

        InteractAction = InputSystem.actions.FindAction("Interact"); // E
    }

    private void Update()
    { 
        if (InteractAction.WasPerformedThisFrame() && LookingAtSomeone)
        {
            inputActions.FindActionMap("UI").Enable();
            inputActions.FindActionMap("Player").Disable();
            istalking = true;
            Cursor.lockState = CursorLockMode.None;
            lastseen.GetComponent<DialogueHandler>().StartTalk();
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.45f, 0), transform.forward * 5, out RaycastHit hit, mask))
        {
            if (hit.collider.gameObject.CompareTag("Interact"))
            {
                //Debug.Log("INTERACTABLE");
                lastseen=hit.collider.gameObject;
                LookingAtSomeone = true;
            }
            else
            {
                //Debug.Log("NON INTERACTABLE");
                lastseen = null;
                LookingAtSomeone = false;
            }
        }
        else
        {
            //Debug.Log("NO HIT");
            lastseen = null;
            LookingAtSomeone = false;
        }
    }   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 0.45f, 0), transform.forward * 3f);
    }
}
