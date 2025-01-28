using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    //[Header("Refernces")]
    Camera cam;
    Rigidbody rb;

    // MOVEMENT
    [SerializeField] float playerHeight = 2;

    public bool isDashPressed = false;
    public bool isJumpPressed = false;
    public Vector2 movementVector;
    Vector3 relativeMovementVector;

    bool lastDashPressed = false;
    bool lastJumpPressed = false;
    Vector3 gravity;

    [Header("Movement Stats")]
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float airStrafe;
    [Tooltip("Slerp rotate the player's velocity towards their desired movement direction to turn faster than acceleration allows.")]
    [SerializeField] float velocitySlerp = 5;
    [Tooltip("Gravity will be multiplied by this value for this character after reaching the apex of their jump or releasing the jump key.")]
    [SerializeField] float fallingGravityModifier = 2;

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

    void Start() {
        gravity = Physics.gravity;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        // Uh, bryce? This is all physics code. It should probably be in fixed update.

        bool isJumpThisFrame = isJumpPressed && isJumpPressed != lastJumpPressed;
        bool isDashThisFrame = isDashPressed && isDashPressed != lastDashPressed;
        lastJumpPressed = isJumpPressed;
        lastDashPressed = isDashPressed;


        if(!canJump && grounded) {
            jumpTimer += Time.deltaTime;
            if(jumpTimer >= jumpCooldown) {
                jumpTimer = 0;
                canJump = true;
            }
        }

        if(!canDash && grounded) {
            dashTimer += Time.deltaTime;
            if(dashTimer >= jumpCooldown) {
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

        float rateOfVelocityChange;

        if(grounded) {

            rateOfVelocityChange = acceleration;

            if(relativeMovementVector.sqrMagnitude > Mathf.Epsilon) {
                if(Vector3.Dot(rb.linearVelocity.normalized, relativeMovementVector) > 0) { //accelerating

                    /* rotate the player's velocity towards their desired movement direction faster
                    than acceleration would normally allow so they don't feel like they're on ice. */
                    float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * velocitySlerp);
                    rb.linearVelocity = Vector3.RotateTowards(rb.linearVelocity, relativeMovementVector.normalized * rb.linearVelocity.magnitude + Vector3.up * rb.linearVelocity.y, blend, 0);

                } else { //decelerating

                    // abruptly stop when trying to run the opposite way.
                    rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, relativeMovementVector.normalized);

                }
            }

            if(isJumpThisFrame) { // jumpInput.WasPressedThisFrame())
                canJump = false;
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, jumpForce, relativeMovementVector.z), ForceMode.Impulse);
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
            } else if(isDashThisFrame) { //dashInput.WasPressedThisFrame())
                canDash = false;
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * dashForce, ForceMode.Impulse);
            }
            rb.linearVelocity += Time.deltaTime * gravity;

        } else { // airborne

            /*if(movementVector.sqrMagnitude > Mathf.Epsilon) {
                rb.linearVelocity += (new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * airStrafe) * Time.deltaTime;
                //rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * airStrafe, ForceMode.Force);
            }*/
            rateOfVelocityChange = airStrafe;

            /* Increase gravity after the climax of our jump or releasing the jump key to make the
            character feel less floaty and allow the player to vary their jump height. */
            if (rb.linearVelocity.y > 0 && isJumpPressed)
                rb.linearVelocity += Time.deltaTime * gravity;
            else
                /* Apparently, putting floats to the left of vectors when multiplying is better for
                performance. I'm sure the compiler just does that for us though. */
                rb.linearVelocity += fallingGravityModifier * Time.deltaTime * gravity;
        }

        // acceleration
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, relativeMovementVector + Vector3.up * rb.linearVelocity.y, rateOfVelocityChange * Time.deltaTime);
    }

    void FixedUpdate() {

        GroundCheck();

    }

    void GroundCheck() {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - (playerHeight / 2), transform.position.z), groundCheckRadius, groundLayer);
        //grounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + 0.1f);
    }

    private void OnDrawGizmos() {
        Vector3 playerPos = transform.position;
        playerPos.y = 0f;
        Gizmos.DrawLine(playerPos, playerPos + relativeMovementVector);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -1 * (playerHeight / 2), 0), groundCheckRadius);
    }
}
