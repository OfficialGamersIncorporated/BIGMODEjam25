using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refernces")]
    Camera cam;
    Rigidbody rb;

    // MOVEMENT
    [SerializeField] float playerHeight = 2;

    //InputAction lateralInput;
    //InputAction dashInput;
    //InputAction jumpInput;

    public bool isDashPressed = false;
    public bool isJumpPressed = false;
    public Vector2 movementVector;
    Vector3 relativeMovementVector;

    bool lastDashPressed = false;
    bool lastJumpPressed = false;

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
        cam = Camera.main;
        //lateralInput = InputSystem.actions.FindAction("Move");
        //dashInput = InputSystem.actions.FindAction("Sprint");
        //jumpInput = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool isJumpThisFrame = isJumpPressed && isJumpPressed != lastJumpPressed;
        bool isDashThisFrame = isDashPressed && isDashPressed != lastDashPressed;
        lastJumpPressed = isJumpPressed;
        lastDashPressed = isDashPressed;


        if (!canJump && grounded)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpCooldown)
            {
                jumpTimer = 0;
                canJump = true;
            }
        }

        if (!canDash && grounded)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= jumpCooldown)
            {
                dashTimer = 0;
                canDash = true;
            }
        }




        //movementVector = lateralInput.ReadValue<Vector2>();

        camForward = cam.transform.forward;
        camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward.normalized;
        Vector3 rightRelative = movementVector.x * camRight.normalized;

        relativeMovementVector = (forwardRelative + rightRelative).normalized * moveSpeed;
        relativeMovementVector.y = 0f;

        if (grounded)
        {

            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, new Vector3(relativeMovementVector.x, rb.linearVelocity.y, relativeMovementVector.z), acceleration * Time.deltaTime);

            if (isJumpThisFrame) // jumpInput.WasPressedThisFrame())
            {
                canJump = false;
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, jumpForce, relativeMovementVector.z), ForceMode.Impulse);
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
            }
            else if (isDashThisFrame) //dashInput.WasPressedThisFrame())
            {
                canDash = false;
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * dashForce, ForceMode.Impulse);
            }
        }
        else
        {
            if (movementVector.sqrMagnitude > Mathf.Epsilon)
            {
                rb.linearVelocity += (new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * airStrafe) * Time.deltaTime;
                //rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * airStrafe, ForceMode.Force);
            }
            
        }


    }

    void FixedUpdate()
    {

        GroundCheck();
        
    }

    void GroundCheck()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - (playerHeight / 2), transform.position.z), groundCheckRadius, groundLayer);
        //grounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + 0.1f);
    }

    private void OnDrawGizmos()
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0f;
        Gizmos.DrawLine(playerPos, playerPos + relativeMovementVector);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -1 * (playerHeight / 2), 0), groundCheckRadius);
    }
}
