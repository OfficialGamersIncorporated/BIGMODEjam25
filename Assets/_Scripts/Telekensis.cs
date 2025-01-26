using UnityEngine;
using UnityEngine.InputSystem;

public class Telekensis : MonoBehaviour
{
    InputAction teleknesisAction;
    Camera cam;
    float raycastDistance = 1000f;

    [SerializeField] LayerMask objectLayer;
    [SerializeField] GameObject heldPart;
    [SerializeField] GameObject teleTarget;
    [SerializeField] float flySpeed;
    Rigidbody partRB;
    bool holdingPart = false;


    void Start()
    {
        teleknesisAction = InputSystem.actions.FindAction("Attack");
        cam = Camera.main;
    }

    void Update()
    {
        if (teleknesisAction.WasPressedThisFrame())
        {
            SelectObject();
        }
        if (teleknesisAction.IsPressed() && heldPart != null && partRB != null)
        {
            //heldPart.transform.position = Vector3.MoveTowards(heldPart.transform.position, teleTarget.transform.position, flySpeed * Time.deltaTime);
            partRB.linearVelocity = Vector3.MoveTowards(partRB.linearVelocity, (teleTarget.transform.position - heldPart.transform.position).normalized * flySpeed, flySpeed * Time.deltaTime);
        }
        if (teleknesisAction.WasReleasedThisFrame())
        {
            if (partRB != null)
            {
                partRB.useGravity = true;
            }
            
            heldPart = null;
            partRB = null;
            holdingPart = false;
        }
    }

    void SelectObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, objectLayer))
        {
            if (hitData.transform.gameObject != null)
            {
                holdingPart = true;
                heldPart = hitData.transform.gameObject;
                partRB = heldPart.GetComponent<Rigidbody>();
                partRB.useGravity = false;
            }
        }
    }
}
