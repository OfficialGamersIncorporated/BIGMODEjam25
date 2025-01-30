using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour {
    public Vector3 pointerPos;
    Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] float raycastDistance = 1000;
    InputAction cameraAction;
    [SerializeField] float flySpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float yOffset = 0.5f;
    [SerializeField] float maxRange = 30;
    public bool matchPlayerHeight = true;

    void Start() {
        cam = Camera.main;
        cameraAction = InputSystem.actions.FindAction("CameraControl");
    }

    void Update() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(matchPlayerHeight) {
            Plane plane = new Plane(Vector3.up, player.transform.position);
            plane.Raycast(ray, out float distance);
            pointerPos = ray.origin + ray.direction * distance + Vector3.up * yOffset;
        } else {
            if(Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, layerMask)) {
                pointerPos = hitData.point + Vector3.up * yOffset;
            }
        }


        // LIMIT POINTERPOS TO UNIT CIRCLE RADIUS OF X FROM PLAYER
        Vector3 toPointerVect = (pointerPos - player.transform.position);
        pointerPos = player.transform.position + Vector3.ClampMagnitude(toPointerVect, maxRange);


        if(pointerPos != null) {
            transform.position = Vector3.MoveTowards(transform.position, pointerPos, Time.deltaTime * flySpeed);
        }
    }
}
