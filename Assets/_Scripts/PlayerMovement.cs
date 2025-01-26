using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    // MOVEMENT
    InputAction movementInput;
    Vector2 movementVector;
    Vector3 relativeMovementVector;

    [Header("Movement Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;


    Vector3 camForward;
    Vector3 camRight;

    void Start()
    {
        movementInput = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementVector = movementInput.ReadValue<Vector2>() * moveSpeed;

        //print("Movement Vector: " +  movementVector);


        camForward = cam.transform.forward;
        camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward.normalized;
        Vector3 rightRelative = movementVector.x * camRight.normalized;

        print("forwardRelative: " + forwardRelative);
        
        relativeMovementVector = (forwardRelative + rightRelative).normalized * moveSpeed;
        relativeMovementVector.y = 0f;

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(relativeMovementVector.x, rb.linearVelocity.y, relativeMovementVector.z);
    }

    private void OnDrawGizmos()
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0f;
        Gizmos.DrawLine(playerPos, playerPos + relativeMovementVector);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + camForward);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + camRight);
    }
}
