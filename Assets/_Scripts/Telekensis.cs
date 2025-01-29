using UnityEngine;
using UnityEngine.InputSystem;

public class Telekensis : MonoBehaviour {
    InputAction teleknesisAction;
    Camera cam;
    float raycastDistance = 1000f;

    [SerializeField] LayerMask objectLayer;
    public GameObject heldPart;
    [SerializeField] GameObject teleTarget;
    [field: SerializeField] public float TeleForce { get; private set; } = 5f;
    [field: SerializeField] public float TeleSpeed { get; private set; } = 5f;
    public float TeleDrag = 0.5f; // why are the lines above so verbose? Why serialized AND public? I smell auto-generated code...
    Rigidbody partRB;
    bool holdingPart = false;


    void Start() {
        teleknesisAction = InputSystem.actions.FindAction("Attack");
        cam = Camera.main;
    }

    void Update() {
        if(teleknesisAction.WasPressedThisFrame()) {
            SelectObject();
        }
        if(teleknesisAction.IsPressed() && heldPart != null && partRB != null) {

            //heldPart.transform.position = Vector3.MoveTowards(heldPart.transform.position, teleTarget.transform.position, flySpeed * Time.deltaTime);

            // you had speed and force mixed up. They're both set to 100 so it didn't make a difference though.
            Vector3 targetVelocity = Vector3.ClampMagnitude(teleTarget.transform.position - heldPart.transform.position, 1) * TeleSpeed; //.normalize
            partRB.linearVelocity = Vector3.MoveTowards(partRB.linearVelocity, targetVelocity, TeleForce * Time.deltaTime);

            // drag
            partRB.linearVelocity = Vector3.MoveTowards(partRB.linearVelocity, Vector3.zero, Time.deltaTime * TeleDrag * partRB.linearVelocity.sqrMagnitude);   

            /*if(Vector3.Magnitude(heldPart.transform.position - teleTarget.transform.position) < 1) {
                partRB.linearVelocity = Vector3.ClampMagnitude(partRB.linearVelocity, Vector3.Magnitude(heldPart.transform.position - teleTarget.transform.position));
            }*/
        }
        if(teleknesisAction.WasReleasedThisFrame()) {
            ReleaseObject();
        }
    }

    void SelectObject() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, objectLayer)) {
            if(hitData.transform.gameObject != null) {
                holdingPart = true;
                heldPart = hitData.transform.gameObject;
                partRB = heldPart.GetComponent<Rigidbody>();
                partRB.useGravity = false;
                partRB.isKinematic = false;
            }
        }
    }
    void ReleaseObject() {
        if(partRB != null) {
            partRB.useGravity = true;
        }

        heldPart = null;
        partRB = null;
        holdingPart = false;
    }

    private void OnDrawGizmos() {
        if(holdingPart) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(heldPart.transform.position, teleTarget.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(heldPart.transform.position, partRB.linearVelocity + heldPart.transform.position);
        }


    }
}
