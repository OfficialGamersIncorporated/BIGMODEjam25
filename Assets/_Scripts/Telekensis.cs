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
    [field: SerializeField] public float TeleForce { get; private set; } = 5f;
    [field: SerializeField] public float TeleSpeed { get; private set; } = 5f;
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
            partRB.linearVelocity = Vector3.MoveTowards(partRB.linearVelocity, (teleTarget.transform.position - heldPart.transform.position).normalized * TeleForce, TeleSpeed * Time.deltaTime);
            if (Vector3.Magnitude(heldPart.transform.position - teleTarget.transform.position) < 1)
            {
                partRB.linearVelocity = Vector3.ClampMagnitude(partRB.linearVelocity, Vector3.Magnitude(heldPart.transform.position - teleTarget.transform.position));
            }
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
                partRB.isKinematic = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (holdingPart)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(heldPart.transform.position, teleTarget.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(heldPart.transform.position, partRB.linearVelocity + heldPart.transform.position);
        }

        
    }
}
