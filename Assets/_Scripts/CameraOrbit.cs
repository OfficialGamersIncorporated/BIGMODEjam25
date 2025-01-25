using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] Transform orbitPoint;
    [SerializeField] float orbitSpeed = 3.0f;
    [SerializeField] float orbitDampening = 10.0f;

    //[Tooltip("Minimum Y Rotation"), Range(-180f, 180.0f)]
    [SerializeField] float minYRotation = -30.0f;
    //[Tooltip("Maximum Y Rotation"), Range(180.0f, 360.0f)]
    [SerializeField] float maxYRotation = 40.0f;

    Vector3 localRotation;

    InputAction mouseMovement;
    Vector2 mouseVector;


    InputAction cancel;
    InputAction camControl;

    void Start()
    {
        mouseMovement = InputSystem.actions.FindAction("Look");
        cancel = InputSystem.actions.FindAction("Cancel");
        camControl = InputSystem.actions.FindAction("CameraControl");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.position = new Vector3(orbitPoint.position.x, orbitPoint.position.y + 1, orbitPoint.position.z);

        mouseVector = mouseMovement.ReadValue<Vector2>();
        //print("Mouse Vector: " + mouseVector);
        print("Mouse " + Mouse.current.delta.ReadValue());

        //if (Mouse.current.delta.ReadValue() != null)
        //{

        //    localRotation.x += Mouse.current.delta.ReadValue().x * orbitSpeed;
        //    localRotation.y += Mouse.current.delta.ReadValue().y * orbitSpeed;

        //    localRotation.y = Mathf.Clamp(localRotation.y, minYRotation, maxYRotation);

        //    Quaternion QT = Quaternion.Euler(localRotation.y * -1, localRotation.x, 0.0f);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDampening);

        //}

        if (Input.GetMouseButton(1))
        {

            localRotation.x += Input.GetAxis("Mouse X") * orbitSpeed;
            localRotation.y += Input.GetAxis("Mouse Y") * orbitSpeed;

            localRotation.y = Mathf.Clamp(localRotation.y, minYRotation, maxYRotation);

            Quaternion QT = Quaternion.Euler(localRotation.y * -1, localRotation.x, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDampening);

        }


        if (cancel.WasPressedThisFrame())
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
    }
}
