using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Analytics.IAnalytic;

public class Telekensis : MonoBehaviour
{
    InputAction teleknesisAction;
    Camera cam;
    float raycastDistance = 1000f;

    [SerializeField] LayerMask objectLayer;
    public GameObject heldPart;
    [SerializeField] FollowMouse teleTarget;
    [field: SerializeField] public float TeleForce { get; private set; } = 5f;
    [field: SerializeField] public float TeleSpeed { get; private set; } = 5f;
    [field: SerializeField] public float TeleDrag { get; private set; } = 0.5f;
    [field: SerializeField] public float MaxRange { get; private set; } = 15;
    Rigidbody partRB;
    //bool holdingPart = false;
    GameObject lastHovered;

    void Start()
    {
        teleknesisAction = InputSystem.actions.FindAction("Attack");
        cam = Camera.main;
        teleTarget.maxRange = MaxRange;
    }

    void Update()
    {
        if (!heldPart)
        {
            GameObject hovered = GetHovered();
            if (hovered != lastHovered)
            {
                if (lastHovered)
                    lastHovered.layer = LayerMask.NameToLayer("Default");
                lastHovered = hovered;
                if (hovered)
                    hovered.layer = LayerMask.NameToLayer("TelekinesisHover");
            }
        }
        teleTarget.matchPlayerHeight = heldPart;
        if (teleknesisAction.WasPressedThisFrame())
        {
            SelectObject();
        }
        if (teleknesisAction.IsPressed() && heldPart != null && partRB != null && !partRB.isKinematic)
        {

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
        if (teleknesisAction.WasReleasedThisFrame())
        {
            ReleaseObject();
        }
    }

    GameObject GetHoveredPrecise()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, objectLayer, QueryTriggerInteraction.Collide)) return null;

        if (hitData.transform.gameObject == null) return null;
        GameObject hitObj = hitData.collider.transform.gameObject;

        if (Vector3.Distance(hitObj.transform.position, PlayerFocusControl.Instance.GetCurrentPlayer().transform.position) > MaxRange)
            return null;

        if (!hitObj.CompareTag("Telekinetic")) return null;

        return hitObj;
    }

    GameObject GetHovered()
    {

        GameObject directlyUnderMouse = GetHoveredPrecise();
        if (directlyUnderMouse) return directlyUnderMouse;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitDataList = Physics.SphereCastAll(ray, 2f, raycastDistance, objectLayer, QueryTriggerInteraction.Collide);

        GameObject closestObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hitData in hitDataList)
        {
            if (hitData.transform.gameObject == null) continue;
            GameObject hitObj = hitData.collider.transform.gameObject;

            if (Vector3.Distance(hitObj.transform.position, PlayerFocusControl.Instance.GetCurrentPlayer().transform.position) > MaxRange)
                continue;

            if (!hitObj.CompareTag("Telekinetic")) continue;

            Vector3 toObjectVect = (hitObj.transform.position - ray.origin);
            Vector3 closestPointOnLine = Vector3.Project(toObjectVect, ray.direction);
            float distance = Vector3.Distance(toObjectVect, closestPointOnLine);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestObj = hitObj;
            }
        }

        return closestObj;
    }
    void SelectObject()
    {
        GameObject hitObj = GetHovered();
        if (!hitObj) return;

        TelekinesisInteractTarget interactTarget = hitObj.GetComponent<TelekinesisInteractTarget>();
        if (interactTarget)
        {
            Rigidbody gotRB = interactTarget.Grab();
            if (gotRB) hitObj = gotRB.gameObject;
            else return;
        }

        partRB = hitObj.GetComponent<Rigidbody>();
        if (!partRB) return;

        //holdingPart = true;
        heldPart = hitObj;
        partRB.useGravity = false;
        partRB.isKinematic = false;
        heldPart.layer = LayerMask.NameToLayer("TelekinesisHover");
    }
    void ReleaseObject()
    {
        if (partRB != null)
        {
            partRB.useGravity = true;
        }

        if (heldPart)
            heldPart.layer = LayerMask.NameToLayer("Default");

        heldPart = null;
        partRB = null;
        //holdingPart = false;
    }

    public void UpgradeTeleForce(float teleForceParam)
    {
        TeleForce = teleForceParam;
    }
    public void UpgradeTeleSpeed(float teleSpeedParam)
    {
        TeleSpeed = teleSpeedParam;
    }
    public void UpgradeTeleMaxRange(float teleMaxRangeParam)
    {
        MaxRange = teleMaxRangeParam;
        teleTarget.maxRange = MaxRange;
    }

    private void OnDrawGizmos()
    {
        if (heldPart /*holdingPart*/)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(heldPart.transform.position, teleTarget.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(heldPart.transform.position, partRB.linearVelocity + heldPart.transform.position);
        }


    }
}
