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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementInput = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = movementInput.ReadValue<Vector2>() * moveSpeed;

        //print("Movement Vector: " +  movementVector);


        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward;
        Vector3 rightRelative = movementVector.x * camRight;

        relativeMovementVector = (forwardRelative + rightRelative).normalized * moveSpeed;
        relativeMovementVector.y = 0f;

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(relativeMovementVector.x, rb.linearVelocity.y, relativeMovementVector.y);
    }
}
