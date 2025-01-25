using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Tooltip("Zoom sensitivity"), Range(0.0f, 10.0f)]
    [SerializeField] float zoomSens = 5.0f;
    [SerializeField] float zoomDampening = 10f;

    [SerializeField] float minY = 2.0f;
    [SerializeField] float maxY = 16.0f;
    [SerializeField] float minZ = -16.0f;
    [SerializeField] float maxZ = -2.0f;

    [Tooltip("Camera Offset")]
    [SerializeField] Vector3 initCameraOffset = new Vector3(0, 10, -10);

    [SerializeField] Vector3 targetZoomPos;

    [SerializeField] GameObject cam;

    InputAction zoomAction;

    void Start()
    {
        targetZoomPos = initCameraOffset;
        zoomAction = InputSystem.actions.FindAction("ScrollWheel");
    }

    void Update()
    {
        if (zoomAction != null)
        {
            targetZoomPos.y -= zoomAction.ReadValue<Vector2>().y * zoomSens;
            targetZoomPos.z += zoomAction.ReadValue<Vector2>().y * zoomSens;

            targetZoomPos.y = Mathf.Clamp(targetZoomPos.y, minY, maxY);
            targetZoomPos.z = Mathf.Clamp(targetZoomPos.z, minZ, maxZ);

            targetZoomPos = new(0.0f, targetZoomPos.y, targetZoomPos.z);
        }

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, targetZoomPos, Time.deltaTime * zoomDampening);

    }
}
