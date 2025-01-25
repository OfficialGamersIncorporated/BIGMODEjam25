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

        print("Movement Vector: " +  movementVector);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementVector.x, rb.linearVelocity.y, movementVector.y);
    }
}
