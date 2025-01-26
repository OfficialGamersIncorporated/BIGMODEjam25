using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public Vector3 pointerPos;
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] float raycastDistance = 1000;
    InputAction cameraAction;
    [SerializeField] float flySpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float yOffset = 0.5f;
    bool holding = false;
    Vector3 holdingPos;

    void Start()
    {
        cameraAction = InputSystem.actions.FindAction("CameraControl");
    }

    void Update()
    {

        if (cameraAction.WasPressedThisFrame())
        {
            holding = true;
            holdingPos = transform.position;
        }
        if (cameraAction.WasReleasedThisFrame())
        {
            holding = false;
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
            pointerPos = holdingPos;
        }



        // LIMIT POINTERPOS TO UNIT CIRCLE RADIUS OF X FROM PLAYER


        if (pointerPos != null)
        {
            pointerPos.y = player.transform.position.y + yOffset;
            transform.position = Vector3.MoveTowards(transform.position, pointerPos, Time.deltaTime * flySpeed);
        }
    }
}
