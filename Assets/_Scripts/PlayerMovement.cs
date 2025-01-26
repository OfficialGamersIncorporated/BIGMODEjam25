using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    // MOVEMENT
    [SerializeField] float playerHeight = 2;

    InputAction lateralInput;
    InputAction dashInput;
    InputAction jumpInput;

    Vector2 movementVector;
    Vector3 relativeMovementVector;
    Vector3 lateralMovementVector;

    [Header("Movement Stats")]
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float airStrafe;

    [Header("Jump")]
    [SerializeField] bool canJump;
    [SerializeField] float jumpTimer;
    [SerializeField] float jumpCooldown;

    [Header("Dash")]
    [SerializeField] bool canDash;
    [SerializeField] float dashTimer;
    [SerializeField] float dashCooldown;

    [Header("Ground Detection")]
    [SerializeField] bool grounded;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;




    Vector3 camForward;
    Vector3 camRight;

    void Start()
    {
        lateralInput = InputSystem.actions.FindAction("Move");
        dashInput = InputSystem.actions.FindAction("Sprint");
        jumpInput = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementVector = lateralInput.ReadValue<Vector2>();

        camForward = cam.transform.forward;
        camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward.normalized;
        Vector3 rightRelative = movementVector.x * camRight.normalized;
        
        relativeMovementVector = (forwardRelative + rightRelative).normalized * moveSpeed;
        relativeMovementVector.y = 0f;

        lateralMovementVector = new Vector3(relativeMovementVector.x, rb.linearVelocity.y, relativeMovementVector.z);

        if (grounded)
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, new Vector3(relativeMovementVector.x, rb.linearVelocity.y, relativeMovementVector.z), acceleration * Time.deltaTime);


            if (jumpInput.WasPressedThisFrame())
            {
                rb.AddRelativeForce(new Vector3(0f, jumpForce, 0f), ForceMode.VelocityChange);
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
            }
            else if (dashInput.WasPressedThisFrame())
            {
                print("DASH");
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * dashForce, ForceMode.VelocityChange);
            }
            else
            {
                //rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z), ForceMode.Acceleration);
            }
        }
        else
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, new Vector3(relativeMovementVector.x / airStrafe, rb.linearVelocity.y, relativeMovementVector.z / airStrafe), acceleration * Time.deltaTime);
            //rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) / 2, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {

        GroundCheck();
        
    }

    void GroundCheck()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - (playerHeight / 2), transform.position.z), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0f;
        Gizmos.DrawLine(playerPos, playerPos + relativeMovementVector);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -(playerHeight / 2), 0), groundCheckRadius);
    }
}
