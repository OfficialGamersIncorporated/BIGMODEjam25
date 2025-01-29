using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public Vector3 pointerPos;
    Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] float raycastDistance = 1000;
    InputAction cameraAction;
    [SerializeField] float flySpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float yOffset = 0.5f;
    bool holding = false;
    Vector3 holdingScreenPos;

    void Start()
    {
        cam = Camera.main;
        cameraAction = InputSystem.actions.FindAction("CameraControl");
    }

    void Update()
    {

        if (cameraAction.WasPressedThisFrame())
        {
            holding = true;
            holdingScreenPos = Input.mousePosition;
        }
        if (cameraAction.WasReleasedThisFrame())
        {
            holding = false;
            holdingScreenPos = Vector3.zero;
        }

        if (!holding)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, layerMask))
            {
                pointerPos = hitData.point;
            }
        }
        else
        {
            Ray ray = cam.ScreenPointToRay(holdingScreenPos);

            if (Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, layerMask))
            {
                pointerPos = hitData.point;
            }
        }



        // LIMIT POINTERPOS TO UNIT CIRCLE RADIUS OF X FROM PLAYER


        if (pointerPos != null)
        {
            pointerPos.y = player.transform.position.y + yOffset;
            transform.position = Vector3.MoveTowards(transform.position, pointerPos, Time.deltaTime * flySpeed);
        }
    }
}
