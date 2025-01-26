using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] Transform orbitPoint;
    [SerializeField] float yOffset;
    [SerializeField] float orbitSpeed = 3.0f;
    [SerializeField] float orbitDampening = 10.0f;

    //[Tooltip("Minimum Y Rotation"), Range(-180f, 180.0f)]
    [SerializeField] float minYRotation = -30.0f;
    //[Tooltip("Maximum Y Rotation"), Range(180.0f, 360.0f)]
    [SerializeField] float maxYRotation = 40.0f;

    Vector3 localRotation;


    InputAction cancel;
    InputAction camControl;

    void Start()
    {
        cancel = InputSystem.actions.FindAction("Cancel");
        camControl = InputSystem.actions.FindAction("CameraControl");

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        transform.position = new Vector3(orbitPoint.position.x, orbitPoint.position.y + yOffset, orbitPoint.position.z);


        // Cam move with mouse locked
        //if (Mouse.current.delta.ReadValue() != null)
        //{

        //    localRotation.x += Mouse.current.delta.ReadValue().x * orbitSpeed;
        //    localRotation.y += Mouse.current.delta.ReadValue().y * orbitSpeed;

        //    localRotation.y = Mathf.Clamp(localRotation.y, minYRotation, maxYRotation);

        //    Quaternion QT = Quaternion.Euler(localRotation.y * -1, localRotation.x, 0.0f);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDampening);

        //}

        // Cam move when holding right click
        if (camControl.IsPressed())
        {

            localRotation.x += Input.GetAxis("Mouse X") * orbitSpeed;
            localRotation.y += Input.GetAxis("Mouse Y") * orbitSpeed;

            localRotation.y = Mathf.Clamp(localRotation.y, minYRotation, maxYRotation);

            Quaternion QT = Quaternion.Euler(localRotation.y * -1, localRotation.x, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDampening);

        }


        if (cancel.WasPressedThisFrame())
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
